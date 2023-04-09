namespace d20Web.SignalRClient
{
    /// <summary>
    /// Arguments for when an object is deleted
    /// </summary>
    public class CombatDeletedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructs a new <see cref="CombatDeletedEventArgs"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="objectID">ID of the deleted object</param>
        public CombatDeletedEventArgs(string campaignID, string objectID)
        {
            CampaignID = campaignID;
            ObjectID = objectID;
        }

        /// <summary>
        /// Gets the ID of the campaign that contains the object
        /// </summary>
        public string CampaignID { get; }
        /// <summary>
        /// Gets the ID of the object that was deleted
        /// </summary>
        public string ObjectID { get; }
    }
}