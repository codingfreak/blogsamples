using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Logic.Core.EventArguments
{
    using Autofac;

    public class ContainerBuilderEventsArgs : EventArgs
    {
        #region constructors and destructors

        public ContainerBuilderEventsArgs(ContainerBuilder containerBuilder)
        {
            ContainerBuilder = containerBuilder;
        }

        #endregion

        #region properties

        public ContainerBuilder ContainerBuilder { get; private set; }

        #endregion
    }
}