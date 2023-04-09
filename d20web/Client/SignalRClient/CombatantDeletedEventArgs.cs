namespace d20Web.SignalRClient
{
    /// <summary>
    /// Arguments for when a combatant is removed from combat
    /// </summary>
    public class CombatantDeletedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatantDeletedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatantID">ID of the combatant that was deleted</param>
        public CombatantDeletedEventArgs(string campaignID, string combatID, IEnumerable<string> combatantIDs)
        {
            CampaignID = campaignID;
            CombatID = combatID;
            CombatantIDs = combatantIDs;
        }

        /// <summary>
        /// Gets the ID of the campaign containing the combat
        /// </summary>
        public string CampaignID { get; }
        /// <summary>
        /// Gets the ID of the combat
        /// </summary>
        public string CombatID { get; }
        public IEnumerable<string> CombatantIDs { get; }
    }
}