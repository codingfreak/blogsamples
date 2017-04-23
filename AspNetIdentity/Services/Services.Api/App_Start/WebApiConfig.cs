using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace codingfreaks.AspNetIdentity.Services.Api
{
    using System.Reflection;

    using Autofac.Integration.WebApi;

    using Logic.Core.Utils;
    using Logic.Shared.TransportModels;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            StartupUtil.AutoFacBuilderReady += (s, e) =>
            {
                e.ContainerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            };            
            StartupUtil.InitLogic();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(StartupUtil.Container);
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
