using d20Web.Models.Combat;

namespace d20Web.SignalRClient
{
    /// <summary>
    /// Arguments for when a combatant's information is updated
    /// </summary>
    public class CombatantUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatantUpdatedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatant">Updated combatant information</param>
        public CombatantUpdatedEventArgs(string campaignID, string combatID, Combatant combatant)
        {
            CampaignID = campaignID;
            CombatID = combatID;
            Combatant = combatant;
        }

        /// <summary>
        /// Gets the ID of the campaign the combat is in
        /// </summary>
        public string CampaignID { get; }
        /// <summary>
        /// Gets the ID of the combat
        /// </summary>
        public string CombatID { get; }
        /// <summary>
        /// Gets the updated combatant information
        /// </summary>
        public Combatant Combatant { get; }
    }
}