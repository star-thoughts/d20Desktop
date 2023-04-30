namespace d20Web.SignalRClient
{
    /// <summary>
    /// Event that is triggered when a new combat is started
    /// </summary>
    public class CombatPrepStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatStartedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        public CombatPrepStartedEventArgs(string campaignID, string combatID)
        {
            CampaignID = campaignID;
            CombatID = combatID;
        }

        /// <summary>
        /// Gets the ID of the campaign containing the combat
        /// </summary>
        public string CampaignID { get; }
        /// <summary>
        /// Gets the ID of the newly created combat
        /// </summary>
        public string CombatID { get; }
    }
}