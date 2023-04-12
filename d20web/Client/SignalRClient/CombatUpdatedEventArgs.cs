using d20Web.Models;

namespace d20Web.SignalRClient
{
    /// <summary>
    /// Arguments for when a combat is updated
    /// </summary>
    public class CombatUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatUpdatedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combat">Updated combat information</param>
        public CombatUpdatedEventArgs(string campaignID, Combat combat)
        {
            CampaignID = campaignID;
            Combat = combat;
        }

        /// <summary>
        /// Gets the ID of the campaign containing the combat
        /// </summary>
        public string CampaignID { get; set; }
        /// <summary>
        /// Gets the updated information for the combat
        /// </summary>
        public Combat Combat { get; set; }
    }
}