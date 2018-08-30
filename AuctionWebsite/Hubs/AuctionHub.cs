using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace AuctionWebsite.Hubs
{
    [HubName("AuctionHub")]
    public class AuctionHub : Hub
    {
        public static IHubContext HubContext { get; } = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();
    }
}