
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(VTIntranetD.Startup))]
namespace VTIntranetD
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            /*app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });

            app.Run(async context =>
            {
                
                var url = context.Request.Path.Value.ToString();

                if (!url.Equals("/Login"))
                {
                    await context.Response.WriteAsync("wait");
                }
               
               
            });
            */
        }   


    }
}