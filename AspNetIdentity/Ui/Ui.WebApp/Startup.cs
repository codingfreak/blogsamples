using codingfreaks.AspNetIdentity.Ui.WebApp;

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Startup))]
namespace codingfreaks.AspNetIdentity.Ui.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
