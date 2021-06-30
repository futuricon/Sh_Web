﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading;

namespace Sh.Web.ViewComponents
{
    [HtmlTargetElement("a", Attributes = ActionAttributeName)]
    [HtmlTargetElement("a", Attributes = ControllerAttributeName)]
    [HtmlTargetElement("a", Attributes = AreaAttributeName)]
    [HtmlTargetElement("a", Attributes = PageAttributeName)]
    [HtmlTargetElement("a", Attributes = PageHandlerAttributeName)]
    [HtmlTargetElement("a", Attributes = FragmentAttributeName)]
    [HtmlTargetElement("a", Attributes = HostAttributeName)]
    [HtmlTargetElement("a", Attributes = ProtocolAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteValuesDictionaryName)]
    [HtmlTargetElement("a", Attributes = RouteValuesPrefix + "*")]
    public class CultureAnchorTagHelper : AnchorTagHelper
    {
        public CultureAnchorTagHelper(IHttpContextAccessor contextAccessor, IHtmlGenerator generator) :
            base(generator)
        {
            this.contextAccessor = contextAccessor;
        }

        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string PageAttributeName = "asp-page";
        private const string PageHandlerAttributeName = "asp-page-handler";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private const string Href = "href";

        private readonly IHttpContextAccessor contextAccessor;
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //var cultureInfo = Thread.CurrentThread.CurrentCulture;
            //RouteValues["culture"] = cultureInfo.ToString();

            var culture = (string)contextAccessor.HttpContext.Request.RouteValues["culture"];

            if (culture != null)
            {
                RouteValues["culture"] = culture;
            }

            base.Process(context, output);
        }
    }
}

