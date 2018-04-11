using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCLogin.Startup))]
namespace MVCLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
