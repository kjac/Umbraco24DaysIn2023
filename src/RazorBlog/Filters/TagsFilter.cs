using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Serialization;

namespace RazorBlog.Filters;

// see https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api/extension-api-for-querying#custom-filter

internal static class TagsFilterConstants
{
    internal const string FilterSpecifier = "tag:";

    internal const string FieldName = "tags";

}

public class TagsContentIndexHandler : IContentIndexHandler
{
    private readonly IJsonSerializer _jsonSerializer;

    public TagsContentIndexHandler(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    // returns the search index field(s) that are (potentially) generated by this index handler
    public IEnumerable<IndexField> GetFields() => new[]
    {
        new IndexField
        {
            FieldName = TagsFilterConstants.FieldName,
            FieldType = FieldType.StringRaw,
            VariesByCulture = false
        }
    };

    // extracts the relevant search index field values (if any) for a content item
    public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
    {
        var tagsValue = content.GetValue<string>("tags");
        // we'll assume that only posts have a "tags" property
        if (tagsValue.IsNullOrWhiteSpace() || tagsValue.DetectIsJson() is false)
        {
            // no tags on this content - it's probably not a post then
            return Array.Empty<IndexFieldValue>();
        }

        // tags are stored as a JSON serialized string array
        var tags = _jsonSerializer.Deserialize<string[]>(tagsValue);
        if (tags?.Any() is not true)
        {
            return Array.Empty<IndexFieldValue>();
        }

        return new[]
        {
            new IndexFieldValue
            {
                FieldName = TagsFilterConstants.FieldName,
                // each post can have multiple tags assigned, so we might store multiple values per field here
                Values = tags.Select(tag => tag.ToLowerInvariant()).ToArray()
            }
        };
    }
}

public class TagsFilterHandler : IFilterHandler
{
    // whether or not this filter can handle a "filter" query part
    public bool CanHandle(string query)
        => query.StartsWith(TagsFilterConstants.FilterSpecifier, StringComparison.OrdinalIgnoreCase);

    // builds the filter option for the "filter" query part
    public FilterOption BuildFilterOption(string filter)
    {
        var fieldValue = filter[TagsFilterConstants.FilterSpecifier.Length..];

        // there might be several values for the filter
        var values = fieldValue.ToLowerInvariant().Split(',');

        return new FilterOption
        {
            FieldName = TagsFilterConstants.FieldName,
            Values = values,
            Operator = FilterOperation.Is
        };
    }
}