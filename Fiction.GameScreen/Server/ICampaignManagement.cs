using Fiction.GameScreen.Monsters;
using Fiction.GameScreen.Players;

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
        /// <summary>
        /// Requests a monster to be deleted from the current campaign
        /// </summary>
        /// <param name="monsterId">ID of the monster</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteMonster(string  monsterId);
        #endregion
        #region Players
        /// <summary>
        /// Creates a new player character
        /// </summary>
        /// <param name="playerCharacter">Character data to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the player character created</returns>
        Task CreatePlayerCharacter(PlayerCharacter playerCharacter, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of all player characters in the campaign
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of player characters</returns>
        Task<PlayerCharacter[]> GetPlayerCharacters(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the data for a player character
        /// </summary>
        /// <param name="id">ID of the character to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Player character data</returns>
        Task<PlayerCharacter> GetPlayerCharacter(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the given player character
        /// </summary>
        /// <param name="id">ID of the character to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchonrous completion</returns>
        Task DeletePlayerCharacter(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the data for a player character
        /// </summary>
        /// <param name="character">Character data to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdatePlayerCharacter(PlayerCharacter character, CancellationToken cancellationToken = default);
        #endregion
    }
}
