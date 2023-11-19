using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace RazorBlog.Extensions;

public static class MediaWithCropsExtensions
{
    public static string? AltText(this MediaWithCrops mediaWithCrops)
        => mediaWithCrops.Content is Image image ? image.AltText : null;
}