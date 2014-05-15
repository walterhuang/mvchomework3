using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcHomework3.Startup))]
namespace MvcHomework3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
