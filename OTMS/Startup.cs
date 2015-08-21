using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OTMS.Startup))]
namespace OTMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
