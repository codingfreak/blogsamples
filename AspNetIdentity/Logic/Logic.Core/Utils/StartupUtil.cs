using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Core.Utils
{
    using System.Collections.Generic;
    using System.Reflection;

    using Autofac;

    using AutoMapper;

    using Data.Core;

    using EventArguments;

    using Shared.Interfaces;
    using Shared.TransportModels;

    using TestRepositories;

    /// <summary>
    /// Provides logic for wiring up often needed components on application startup.
    /// </summary>
    public static class StartupUtil
    {

        /// <summary>
        /// Is fired before the AutoFac container is beeing built so that the receiver can do
        /// custom stuff with AutoFac builder.
        /// </summary>
        public static event EventHandler<ContainerBuilderEventsArgs> AutoFacBuilderReady;

        #region methods

        public static void InitLogic(bool runsUnderTest = false)
        {
            // initialize AutoMapper
            Mapper.Initialize(
                cfg =>
                {                    
                    cfg.CreateMap<User, UserTransportModel>();
                    cfg.CreateMap<UserTransportModel, User>();                    
                });            
            // initialize AutoFac as DI
            var builder = new ContainerBuilder();
            if (runsUnderTest)
            {
                var coreLogic = Assembly.GetExecutingAssembly();
                builder.RegisterAssemblyTypes(coreLogic).Where(t => t.Name.Contains("Test") && t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            }
            else
            {
                var coreLogic = Assembly.GetExecutingAssembly();
                builder.RegisterAssemblyTypes(coreLogic).Where(t => !t.Name.Contains("Test") && t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            }
            // give the caller additional chance to do something with the builder
            AutoFacBuilderReady?.Invoke(null, new ContainerBuilderEventsArgs(builder));
            Container = builder.Build();
        }

        #endregion

        #region properties

        /// <summary>
        /// The static AutoFac container for DI.
        /// </summary>
        public static IContainer Container { get; private set; }

        #endregion
    }
}