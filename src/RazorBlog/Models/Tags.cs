namespace Umbraco.Cms.Web.Common.PublishedModels;

public partial class Tags
{
    public string Tag { get; set; }

    public Post[] Posts { get; set; }
}