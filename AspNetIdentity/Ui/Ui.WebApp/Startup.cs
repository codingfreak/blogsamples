using codingfreaks.AspNetIdentity.Ui.WebApp;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace codingfreaks.AspNetIdentity.Ui.WebApp
{
    using System;
    using System.Linq;

    using Owin;

    public partial class Startup
    {
        #region methods

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        #endregion
    }
}