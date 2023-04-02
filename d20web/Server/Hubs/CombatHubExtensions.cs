using d20Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace d20Web.Hubs
{
    /// <summary>
    /// Extension methods for reporting combat information via SignalR
    /// </summary>
    public static class CombatHubExtensions
    {
        public static Task CombatCreated(this IHubContext<CampaignHub> hub, string campaignID, string combatID, string combatName)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatCreated, campaignID, combatID, combatName);
        }
        public static Task CombatUpdated(this IHubContext<CampaignHub> hub, string campaignID, Combat combat)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatUpdated, campaignID, combat);
        }

        public static Task CombatantCreated(this IHubContext<CampaignHub> hub, string campaignID, IEnumerable<string> combatantIDs)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantCreated, campaignID, combatantIDs);
        }

        public static Task CombatantUpdated(this IHubContext<CampaignHub> hub, string campaignID, Combatant combatant)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantUpdated, campaignID, combatant);
        }
    }
}
