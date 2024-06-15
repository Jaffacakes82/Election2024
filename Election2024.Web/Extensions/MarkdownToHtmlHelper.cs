using Markdig;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Election2024.Web.Extensions;

public static class MarkdownToHtmlHelper
{
    public static IHtmlContent MarkdownToHtml(this IHtmlHelper htmlHelper, string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return htmlHelper.Raw(string.Empty);
        }

        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(markdown, pipeline);
        return htmlHelper.Raw(html);
    }
}
