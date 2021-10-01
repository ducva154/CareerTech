using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CareerTech.Startup))]
namespace CareerTech
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
