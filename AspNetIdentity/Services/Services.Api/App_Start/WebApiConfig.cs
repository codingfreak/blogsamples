namespace codingfreaks.AspNetIdentity.Services.Api
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;

    using Autofac.Integration.WebApi;

    using Logic.Core.Utils;

    /// <summary>
    /// Central logic to configure Web API on this project.
    /// </summary>
    public static class WebApiConfig
    {
        #region methods

        /// <summary>
        /// Is called by the <see cref="WebApiApplication" /> to initialize Web API.
        /// </summary>
        /// <param name="config">The HTTP configuration to use.</param>
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
                "DefaultApi",
                "api/{controller}/{id}",
                new
                {
                    id = RouteParameter.Optional
                });
        }

        #endregion
    }
}