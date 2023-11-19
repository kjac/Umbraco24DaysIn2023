using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace RazorBlog.Controllers;

public abstract class ControllerBase<TModel> : RenderController
    where TModel : PublishedContentModel
{
    private readonly ILogger<RenderController> _logger;
    private readonly IApiContentQueryService _apiContentQueryService;
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    protected abstract IActionResult Index(TModel model);
    
    protected ControllerBase(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IApiContentQueryService apiContentQueryService,
        IPublishedSnapshotAccessor publishedSnapshotAccessor)
        : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _logger = logger;
        _apiContentQueryService = apiContentQueryService;
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
    }

    public override IActionResult Index()
        => CurrentPage is TModel model ? Index(model) : Render();

    protected IActionResult Render() => base.Index();
    
    protected T[] QueryContent<T>(string? fetch, IEnumerable<string> filters, IEnumerable<string> sorts, int skip, int take)
        where T : IPublishedContent
    {
        // use the Delivery API query service to find content matching the passed parameters.
        // this may seem kinda silly; with ModelsBuilder models we can query content in a strongly typed manner.
        // however, if the dataset grows large (a whole lot of content), that strongly typed (in-memory) query might
        // not be super performant.
        // even moreso, if we were querying for something that was expensive to calculate (some kind of composite value),
        // the in-memory query would quickly become sluggish and resource consuming. by using the Delivery API query
        // service, we're always leveraging pre-calculated (indexed) values for our query.
        var queryResult = _apiContentQueryService.ExecuteQuery(fetch, filters, sorts, skip, take);

        if (queryResult.Success is false)
        {
            _logger.LogWarning(
                queryResult.Exception,
                "Could not execute the query (fetch = {fetch}) against the Delivery API index - the returned status was: {status}",
                fetch,
                queryResult.Status);

            return Array.Empty<T>();
        }
        
        var publishedContentCache = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Content
                                    ?? throw new InvalidOperationException("Could not obtain the published content cache");

        return queryResult.Result.Items.Select(publishedContentCache.GetById).WhereNotNull().OfType<T>().ToArray();        
    }
}