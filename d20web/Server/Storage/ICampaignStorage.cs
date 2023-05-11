using d20Web.Models;
using d20Web.Models.Bestiary;

namespace d20Web.Storage
{
    /// <summary>
    /// Main interface for storing campaign information
    /// </summary>
    public interface ICampaignStorage
    {
        /// <summary>
        /// Creates a campaign
        /// </summary>
        /// <param name="name">Name of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the campaign that was created</returns>
        Task<string> CreateCampaign(string name, CancellationToken cancellationToken);
        /// <summary>
        /// Gets a collection of campaigns in the system
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of campaign information</returns>
        Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the information for a campaign
        /// </summary>
        /// <param name="campaignId">ID of the campaign to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Campaign information retrieved</returns>
        Task<Campaign> GetCampaign(string campaignId, CancellationToken cancellationToken);
        #region Monsters
        /// <summary>
        /// Creates a monster in the bestiary
        /// </summary>
        /// <param name="campaignID">ID of the campaign to put the monster in</param>
        /// <param name="monster">Monster to add to the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the monster that was added</returns>
        Task<string> CreateMonster(string campaignID, Monster monster, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the monster with the given ID
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="monsterID">ID of the monster to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Monster retrieved from DB</returns>
        Task<Monster> GetMonster(string campaignID, string monsterID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the monster
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="monster">Monster information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateMonster(string campaignID, Monster monster, CancellationToken cancellationToken);
        /// <summary>
        /// Deletes the given monster
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="id">ID of the monster to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteMonster(string campaignID, string id, CancellationToken cancellationToken);
        #endregion
    }
}
