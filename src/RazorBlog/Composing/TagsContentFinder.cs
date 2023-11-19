using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace RazorBlog.Composing;

public class TagsContentFinder : IContentFinder
{
    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

    public TagsContentFinder(IPublishedSnapshotAccessor publishedSnapshotAccessor)
        => _publishedSnapshotAccessor = publishedSnapshotAccessor;

    public Task<bool> TryFindContent(IPublishedRequestBuilder request)
    {
        if (request.Uri.PathAndQuery.StartsWith("/tags/", StringComparison.OrdinalIgnoreCase) is false)
        {
            return Task.FromResult(false);
        }

        var publishedSnapshot = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot();
        request.SetPublishedContent(publishedSnapshot.Content!.GetAtRoot().First().FirstChild<Tags>()!);
        
        return Task.FromResult(true);
    }
}