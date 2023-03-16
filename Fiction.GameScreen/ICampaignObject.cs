namespace Fiction.GameScreen
{
    /// <summary>
    /// Base interface for objects in a campaign
    /// </summary>
    public interface ICampaignObject
    {
        /// <summary>
        /// Gets the name of this campaign object
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the campaign this object is part of
        /// </summary>
        CampaignSettings Campaign { get; }
        /// <summary>
        /// Gets the ID of this object
        /// </summary>
        int Id { get; }
    }
}