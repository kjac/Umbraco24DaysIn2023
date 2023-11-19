using Microsoft.AspNetCore.Mvc.Filters;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace RazorBlog.Composing;

public class RoutingComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddContentFinder<TagsContentFinder>();
    }

    private IPublishedContent FindContent(ActionExecutingContext actionExecutingContext)
    {
        var umbracoContextAccessor = actionExecutingContext.HttpContext.RequestServices
            .GetRequiredService<IUmbracoContextAccessor>();
        var umbracoContext = umbracoContextAccessor.GetRequiredUmbracoContext();

        return umbracoContext.Content!.GetAtRoot().First().FirstChild<Tags>()!;
    }    
}