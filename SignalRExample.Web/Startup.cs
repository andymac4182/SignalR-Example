using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRExample.Web.Startup))]
namespace SignalRExample.Web
{
    using Microsoft.AspNet.SignalR;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //GlobalHost.HubPipeline.AddModule(new LoggingPipelineModule());
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
