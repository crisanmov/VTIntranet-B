using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VTIntranetD.Startup))]
namespace VTIntranetD
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}