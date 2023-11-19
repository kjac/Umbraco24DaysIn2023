using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace RazorBlog.Controllers;

public class HomeController : ControllerBase<Home>
{
    public HomeController(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IApiContentQueryService apiContentQueryService,
        IPublishedSnapshotAccessor publishedSnapshotAccessor)
        : base(logger, compositeViewEngine, umbracoContextAccessor, apiContentQueryService, publishedSnapshotAccessor)
    {
    }

    protected override IActionResult Index(Home home)
    {
        // NOTE: we should create a proper view model for the view instead of extending the ModelsBuilder one.
        //       for the sake of this demo, we'll make do with this quick-and-dirty fix.
        home.Posts = QueryContent<Post>(
            fetch: "children:posts",
            filters: Array.Empty<string>(),
            sorts: new[] { "updateDate:desc" }, 
            skip: 0,
            take: 10);

        return Render();
    }
}