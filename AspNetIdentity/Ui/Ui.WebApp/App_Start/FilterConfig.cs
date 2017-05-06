namespace codingfreaks.AspNetIdentity.Ui.WebApp
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class FilterConfig
    {
        #region methods

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        #endregion
    }
}