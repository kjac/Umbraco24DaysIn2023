using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using RazorBlog.Filters;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace RazorBlog.Controllers;

public class AuthorController : ControllerBase<Author>
{
    public AuthorController(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IApiContentQueryService apiContentQueryService,
        IPublishedSnapshotAccessor publishedSnapshotAccessor)
        : base(logger, compositeViewEngine, umbracoContextAccessor, apiContentQueryService, publishedSnapshotAccessor)
    {
    }

    protected override IActionResult Index(Author author)
    {
        // NOTE: we should create a proper view model for the view instead of extending the ModelsBuilder one.
        //       for the sake of this demo, we'll make do with this quick-and-dirty fix.
        author.Posts = QueryContent<Post>(
            fetch: "children:posts",
            filters: new[] { $"{AuthorFilterConstants.FilterSpecifier}{author.Key}" },
            sorts: new[] { "updateDate:desc" }, 
            skip: 0,
            take: 10);

        return Render();
    }    
}