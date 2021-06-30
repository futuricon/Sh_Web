using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sh.Web.ViewComponents
{
    /// <summary>
    /// Tag helper to render pagination UI.
    /// Requires client-side Bootstrap 4 files to display correctly.
    /// </summary>
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {

        /// <summary>
        /// TotalPages - number of pages, default value is 10
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// PageIndex - number of the current page, default value is 1
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// PagePath - page path, default value is <c>"./Index"</c>
        /// </summary>
        public string PagePath { get; set; }

        /// <summary>
        /// PageHandler - page handler name to pass in query of the URL, default value is <c>"pageIndex"</c>
        /// </summary>
        public string PageHandler { get; set; }

        /// <summary>
        /// PageFragment - page fragment name to pass in query of the URL, default value is <c>null</c>
        /// </summary>
        public string PageFragment { get; set; }

        public PaginationTagHelper()
        {
            PageHandler = "pageIndex";
            //PagePath = "./blog";
            TotalPages = 10;
            PageIndex = 1;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var prevDisabled = PageIndex > 1 ? "" : "hidden";
            var nextDisabled = PageIndex < TotalPages ? "" : "hidden";

            output.TagName = "pagination";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent("<ul class='pagination'>");
            output.Content.AppendHtml($"<li class='page-item' {prevDisabled}>" +
                $"<a href='{PagePath}?{PageHandler}={PageIndex - 1}#{PageFragment}' class='page-link' aria-label='Previous'><span class='fas fa-arrow-left'></span></a></li>");

            if (TotalPages <= 10)
            {
                for (var i = 1; i <= TotalPages; i++)
                {
                    var activeClassName = i == PageIndex ? "active" : "";
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a href='{PagePath}?{PageHandler}={i}#{PageFragment}' class='page-link'>{i}</a></li>");
                }
            }
            else
            {
                var activeClassName = PageIndex == 1 ? "active" : "";

                if (PageIndex - 4 > 1)
                {
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a href='{PagePath}?{PageHandler}=1#{PageFragment}' class='page-link'>1</a></li>");
                    output.Content.AppendHtml("<li class='page-item disabled' style='pointer-events: none;'><a class='page-link'>...</a></li>");
                }

                for (var i = PageIndex - 4; i <= PageIndex + 4; i++)
                {
                    if (i > TotalPages)
                        break;

                    if (i <= 0)
                        continue;
                    activeClassName = i == PageIndex ? "active" : "";
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a href='{PagePath}?{PageHandler}={i}#{PageFragment}' class='page-link'>{i}</a></li>");
                }

                if (TotalPages - PageIndex > 4)
                {
                    activeClassName = PageIndex == TotalPages ? "active" : "";
                    output.Content.AppendHtml("<li class='page-item disabled' style='pointer-events: none;'><a class='page-link'>...</a></li>");
                    output.Content.AppendHtml($"<li class='page-item {activeClassName}'><a href='{PagePath}?{PageHandler}={TotalPages}#{PageFragment}' class='page-link'>{TotalPages}</a></li>");
                }
            }

            output.Content.AppendHtml($"<li class='page-item' {nextDisabled}><a href='{PagePath}?{PageHandler}={PageIndex + 1}#{PageFragment}' class='page-link' aria-label='Next'><span class='fas fa-arrow-right'></span></a></li>");
            output.Content.AppendHtml("</ul>");
        }
    }
}
