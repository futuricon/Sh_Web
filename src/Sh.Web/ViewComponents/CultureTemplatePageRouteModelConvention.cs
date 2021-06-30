using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Threading;

namespace Sh.Web.ViewComponents
{
    public class CultureTemplatePageRouteModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var selectorCount = model.Selectors.Count;

            for (var i = 0; i < selectorCount; i++)
            {
                var selector = model.Selectors[i];
                var cultureString = "{culture=" + cultureInfo + "}";
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Order = -1,
                        Template = AttributeRouteModel.CombineTemplates(
                            cultureString, selector.AttributeRouteModel.Template),
                    }
                });
            }
        }
    }
}
