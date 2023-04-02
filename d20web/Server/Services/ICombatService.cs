using d20Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace d20Web.Services
{
    /// <summary>
    /// Interface for managing combat
    /// </summary>
    public interface ICombatService
    {
        /// <summary>
        /// Creates a new combat in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create the combat in</param>
        /// <param name="name">Name of the combat to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat that was created</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        Task<string> CreateCombat(string campaignID, string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets information for a combat
        /// </summary>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        Task<Combat> GetCombat(string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Creates a combatant with the given statistics
        /// </summary>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        Task<IEnumerable<string>> CreateCombatant(string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        Task<Combatant> GetCombatant(string combatID, string combatantID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        Task<IEnumerable<Combatant>> GetCombatants(string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the stats for a combatant
        /// </summary>
        /// <param name="campaignID">Campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatant">Combatant details to use for the update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatant(string campaignID, string combatID, Combatant combatant, CancellationToken cancellationToken = default);
    }
}