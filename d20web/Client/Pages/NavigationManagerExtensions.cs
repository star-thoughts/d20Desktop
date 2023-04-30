using Microsoft.AspNetCore.Components;

namespace d20Web.Pages
{
    /// <summary>
    /// Extension methods for navigating to pages
    /// </summary>
    public static class NavigationManagerExtensions
    {
        /// <summary>
        /// The URI for navigating to a campaign page
        /// </summary>
        /// <remarks>
        /// {0} is the ID of the campaign.
        /// </remarks>
        public const string CampaignPageUri = "/campaign/{0}";
        /// <summary>
        /// The URI for navigating to a combat page
        /// </summary>
        public const string CombatPageUri = "/campaign/{0}/combat/{1}";
        /// <summary>
        /// The URI for navigating to a combat prep page
        /// </summary>
        public const string CombatPrepPageUri = "/campaign/{0}/combat/prep/{1}";

        /// <summary>
        /// Navigates to the campaign page for the given campaign ID
        /// </summary>
        /// <param name="manager">Navigation manager to use</param>
        /// <param name="campaignID">ID of the campaign to navigate to</param>
        public static void NavigateToCampaign(this NavigationManager manager, string campaignID)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));

            manager.NavigateTo(string.Format(CampaignPageUri, campaignID));
        }
        /// <summary>
        /// Navigates to the combat page for the given combat
        /// </summary>
        /// <param name="manager">Navigation manager to use</param>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat to navigate to</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NavigateToCombat(this NavigationManager manager, string campaignID, string combatID)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            manager.NavigateTo(string.Format(CombatPageUri, campaignID, combatID));
        }
        /// <summary>
        /// Navigates to the combat prep page for the given combat
        /// </summary>
        /// <param name="manager">Navigation manager to use</param>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat to navigate to</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NavigateToCombatPrep(this NavigationManager manager, string campaignID, string combatID)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            manager.NavigateTo(string.Format(CombatPrepPageUri, campaignID, combatID));
        }
    }
}
