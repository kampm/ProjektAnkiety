using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(testankiety.Startup))]
namespace testankiety
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
