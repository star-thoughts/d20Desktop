using d20Web.Models.Bestiary;
using Microsoft.AspNetCore.SignalR;

namespace d20Web.Hubs
{
    static class CampaignHubExtensions
    {
        #region Campaigns
        public static Task CampaignCreated(this IHubContext<CampaignHub> hub, string campaignID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CampaignCreated, campaignID);
        }
        #endregion
        #region Monsters
        public static Task MonsterCreated(this IHubContext<CampaignHub> hub, string campaignID, string monsterID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.MonsterCreated, campaignID, monsterID);
        }

        public static Task MonsterUpdated(this IHubContext<CampaignHub> hub, string campaignID, Monster monster)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.MonsterUpdated, campaignID, monster);
        }

        public static Task MonsterDeleted(this IHubContext<CampaignHub> hub, string campaignID, string monsterID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.MonsterDeleted, campaignID, monsterID);
        }


        #endregion
    }
}
