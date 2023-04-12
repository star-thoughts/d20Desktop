namespace d20Web.SignalRClient
{
    /// <summary>
    /// Arguments for when a combatant is added to a combat
    /// </summary>
    public class CombatantCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatantCreatedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign that the combat is in</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatantIDs">IDs of the added combatants</param>
        public CombatantCreatedEventArgs(string campaignID, string combatID, IEnumerable<string> combatantIDs)
        {
            CampaignID = campaignID;
            CombatID = combatID;
            CombatantIDs = combatantIDs;
        }

        /// <summary>
        /// Gets the ID of the campaign containing the combat
        /// </summary>
        public string CampaignID { get; set; }
        /// <summary>
        /// Gets the ID of the combat
        /// </summary>
        public string CombatID { get; }
        /// <summary>
        /// Gets the IDs of the combatants that were added
        /// </summary>
        public IEnumerable<string> CombatantIDs { get; }
    }
}