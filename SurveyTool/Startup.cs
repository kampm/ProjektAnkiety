using Microsoft.Owin;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(SurveyTool.Startup))]
namespace SurveyTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API")).EnableSwaggerUi();
            //app.Use(config);
            ConfigureAuth(app);
        }
    }
}
