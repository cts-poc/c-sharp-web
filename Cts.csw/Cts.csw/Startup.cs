using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cts.csw.Startup))]
namespace Cts.csw
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
