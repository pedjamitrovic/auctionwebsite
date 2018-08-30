using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(AuctionWebsite.Startup))]
namespace AuctionWebsite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}