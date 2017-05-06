namespace codingfreaks.AspNetIdentity.Ui.WebApp
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        #region methods

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }

        #endregion
    }
}