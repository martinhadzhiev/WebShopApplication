using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebShop.App.Startup))]
namespace WebShop.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
