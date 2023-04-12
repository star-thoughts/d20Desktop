using Microsoft.AspNetCore.SignalR;

namespace d20Web.Hubs
{
    static class CampaignHubExtensions
    {
        public static Task CampaignCreated(this IHubContext<CampaignHub> hub, string campaignID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CampaignCreated, campaignID);
        }
    }
}
