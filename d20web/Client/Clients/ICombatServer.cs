using d20Web.Models;

namespace d20Web.Clients
{
    /// <summary>
    /// Interface for communicating with a combat server
    /// </summary>
    public interface ICombatServer
    {
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
    }
}