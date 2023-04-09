namespace d20Web.SignalRClient
{
    /// <summary>
    /// Event that is triggered when a new combat is started
    /// </summary>
    public class CombatStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatStartedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatName">Name of the combat that was started</param>
        public CombatStartedEventArgs(string campaignID, string combatID, string combatName)
        {
            CampaignID = campaignID;
            CombatID = combatID;
            CombatName = combatName;
        }

        /// <summary>
        /// Gets the ID of the campaign containing the combat
        /// </summary>
        public string CampaignID { get; }
        /// <summary>
        /// Gets the ID of the newly created combat
        /// </summary>
        public string CombatID { get; }
        /// <summary>
        /// Gets the name of the combat
        /// </summary>
        public string CombatName { get; }
    }
}