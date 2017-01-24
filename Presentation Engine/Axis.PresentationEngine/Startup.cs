using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Axis.PresentationEngine.Startup))]
namespace Axis.PresentationEngine
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
