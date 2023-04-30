using d20Web.Models;

namespace d20Web.Clients
{
    /// <summary>
    /// Interface for communicating with a combat server
    /// </summary>
    public interface ICombatServer
    {
        #region Combat Prep
        /// <summary>
        /// Gets a list of combat preps in the campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat prep is in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>List of combat preps</returns>
        Task<IEnumerable<CombatListData>> GetCombatPreps(string campaignID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets combat prep information
        /// </summary>
        /// <param name="campaignID">ID of the campaign that contains the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        Task<CombatPrep> GetCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets details about a combat prep's combatants
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat's combatants to retrieve</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        Task<IEnumerable<CombatantPreparer>> GetCombatantPreparers(string campaignID, string combatID, CancellationToken cancellationToken = default);
        #endregion
        #region Combat
        /// <summary>
        /// Gets a list of combats in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        Task<IEnumerable<CombatListData>> GetCombats(string campaignID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets details about a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to retrieve</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        Task<Combat> GetCombat(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets details about a combat's combatants
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat's combatants to retrieve</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        Task<IEnumerable<Combatant>> GetCombatants(string campaignID, string combatID, CancellationToken cancellationToken = default);
        #endregion
    }
}