namespace codingfreaks.AspNetIdentity.Data.Core
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public static class ContextUtil
    {
        #region properties

        public static IdentityEntities Context
        {
            get
            {
                var context = new IdentityEntities();
                context.Configuration.LazyLoadingEnabled = true;
                context.Database.Log = msg => Trace.TraceInformation(msg);
                return context;
            }
        }

        #endregion
    }
}