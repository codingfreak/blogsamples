using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Ui.WebApp
{
    using System.Web.Mvc;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
