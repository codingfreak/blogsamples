namespace codingfreaks.AspNetIdentity.Ui.WebApp
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using AutoMapper;

    using Logic.Shared.TransportModels;
    using Logic.Ui.Models;

    public class MvcApplication : HttpApplication
    {
        #region methods

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // initialize AutoMapper
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<ApplicationUser, UserTransportModel>();
                    cfg.CreateMap<UserTransportModel, ApplicationUser>();
                });
        }

        #endregion
    }
}