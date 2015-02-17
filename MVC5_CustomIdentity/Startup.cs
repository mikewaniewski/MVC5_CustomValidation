using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5_CustomIdentity.Startup))]
namespace MVC5_CustomIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
