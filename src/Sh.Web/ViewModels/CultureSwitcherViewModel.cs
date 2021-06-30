using System.Collections.Generic;
using System.Globalization;

namespace Sh.Web.ViewModels
{
    public class CultureSwitcherViewModel
    {
        public CultureInfo CurrentUICulture { get; set; }
        public List<CultureInfo> SupportedCultures { get; set; }
    }
}
