using Fiction.GameScreen.Monsters;

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

        #region Bestiary
        /// <summary>
        /// Requests a monster be created in the current campaign
        /// </summary>
        /// <param name="monster">Monster to create</param>
        /// <returns>Task for asynchronous completion</returns>
        Task CreateMonster(Monster monster);
        /// <summary>
        /// Requests a monster be updated in the current campaign
        /// </summary>
        /// <param name="monster">Monster to udpated</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateMonster(Monster monster);
        #endregion
    }
}
