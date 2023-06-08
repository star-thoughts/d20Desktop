using d20Web.Models.Combat;
using Microsoft.AspNetCore.SignalR;

namespace d20Web.Hubs
{
    /// <summary>
    /// Extension methods for reporting combat information via SignalR
    /// </summary>
    public static class CombatHubExtensions
    {
        #region Combat Prep
        public static Task CombatPrepCreated(this IHubContext<CampaignHub> hub, string campaignID, string combatID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatPrepCreated, campaignID, combatID);
        }
        public static Task CombatPrepDeleted(this IHubContext<CampaignHub> hub, string campaignID, string combatID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatPrepDeleted, campaignID, combatID);
        }
        public static Task CombatantPrepCreated(this IHubContext<CampaignHub> hub, string campaignID, string combatID, IEnumerable<string> combatantIDs)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantPrepCreated, campaignID, combatID, combatantIDs);
        }

        public static Task CombatantPrepUpdated(this IHubContext<CampaignHub> hub, string campaignID, string combatID, CombatantPreparer combatant)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantPrepUpdated, campaignID, combatID, combatant);
        }

        public static Task CombatantPrepsDeleted(this IHubContext<CampaignHub> hub, string campaignID, string combatID, IEnumerable<string> combatantIDs)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantPrepsDeleted, campaignID, combatID, combatantIDs);
        }
        #endregion
        #region Combat
        public static Task CombatCreated(this IHubContext<CampaignHub> hub, string campaignID, string combatID, string combatName)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatCreated, campaignID, combatID, combatName);
        }
        public static Task CombatDeleted(this IHubContext<CampaignHub> hub, string campaignID, string combatID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatDeleted, campaignID, combatID);
        }
        public static Task CombatUpdated(this IHubContext<CampaignHub> hub, string campaignID, Combat combat)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatUpdated, campaignID, combat);
        }

        public static Task CombatantCreated(this IHubContext<CampaignHub> hub, string campaignID, string combatID, IEnumerable<string> combatantIDs)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantCreated, campaignID, combatID, combatantIDs);
        }

        public static Task CombatantUpdated(this IHubContext<CampaignHub> hub, string campaignID, string combatID, Combatant combatant)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantUpdated, campaignID, combatID, combatant);
        }

        public static Task CombatantsDeleted(this IHubContext<CampaignHub> hub, string campaignID, string combatID, IEnumerable<string> combatantIDs)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatantsDeleted, campaignID, combatID, combatantIDs);
        }
        #endregion
        #region Scenarios
        public static Task CombatScenarioCreated(this IHubContext<CampaignHub> hub, string campaignID, string CombatScenarioID, string CombatScenarioName)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatScenarioCreated, campaignID, CombatScenarioID, CombatScenarioName);
        }
        public static Task CombatScenarioDeleted(this IHubContext<CampaignHub> hub, string campaignID, string CombatScenarioID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatScenarioDeleted, campaignID, CombatScenarioID);
        }
        public static Task CombatScenarioUpdated(this IHubContext<CampaignHub> hub, string campaignID, CombatScenario CombatScenario)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.CombatScenarioUpdated, campaignID, CombatScenario);
        }
        public static Task ScenarioCombatantCreated(this IHubContext<CampaignHub> hub, string campaignID, string ScenarioCombatantID, string ScenarioCombatantName)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.ScenarioCombatantCreated, campaignID, ScenarioCombatantID, ScenarioCombatantName);
        }
        public static Task ScenarioCombatantDeleted(this IHubContext<CampaignHub> hub, string campaignID, string ScenarioCombatantID)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.ScenarioCombatantDeleted, campaignID, ScenarioCombatantID);
        }
        public static Task ScenarioCombatantUpdated(this IHubContext<CampaignHub> hub, string campaignID, CombatantTemplate ScenarioCombatant)
        {
            if (hub == null)
                throw new ArgumentNullException(nameof(hub));

            return hub.Clients.All.SendAsync(Constants.ScenarioCombatantUpdated, campaignID, ScenarioCombatant);
        }

        #endregion
    }
}
