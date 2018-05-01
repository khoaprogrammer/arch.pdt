using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CIF.Startup))]
namespace CIF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
