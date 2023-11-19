using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using RazorBlog.Filters;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace RazorBlog.Controllers;

public class TagsController : ControllerBase<Tags>
{
    public TagsController(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IApiContentQueryService apiContentQueryService,
        IPublishedSnapshotAccessor publishedSnapshotAccessor)
        : base(logger, compositeViewEngine, umbracoContextAccessor, apiContentQueryService, publishedSnapshotAccessor)
    {
    }

    protected override IActionResult Index(Tags tags)
    {
        var tag = Request.Path.ToString().Split(Constants.CharArrays.ForwardSlash).Last();

        // NOTE: we should create a proper view model for the view instead of extending the ModelsBuilder one.
        //       for the sake of this demo, we'll make do with this quick-and-dirty fix.
        tags.Tag = tag.ToFirstUpperInvariant();
        tags.Posts = QueryContent<Post>(
            fetch: "children:posts",
            filters: new[] { $"{TagsFilterConstants.FilterSpecifier}{tag}" },
            sorts: new[] { "updateDate:desc" },
            skip: 0,
            take: 10);

        return Render();
    }
}