namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Interface for communicating with a campaign server
    /// </summary>
    public interface ICampaignManagement
    {
        /// <summary>
        /// Gets the interface to use for combat management
        /// </summary>
        public ICombatManagement Combat { get; }

        /// <summary>
        /// Requests a campaign be created on the server
        /// </summary>
        /// <param name="name">Name of the campaign to create</param>
        /// <returns>ID of the campaign that was created</returns>
        Task<string> CreateCampaign(string name);
    }
}
