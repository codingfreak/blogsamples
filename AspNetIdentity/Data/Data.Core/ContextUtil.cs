using System;
using System.Linq;

namespace codingfreaks.AspNetIdentity.Data.Core
{
    using System.Diagnostics;

    public static class ContextUtil
    {
        #region properties

        public static IdentityEntities Context
        {
            get
            {
                var context = new IdentityEntities();
                context.Configuration.LazyLoadingEnabled = false;
                context.Database.Log = msg => Trace.TraceInformation(msg);
                return context;
            }
        }

        #endregion
    }
}