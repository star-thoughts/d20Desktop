﻿using d20Web.Models;
using System.ComponentModel.DataAnnotations;

namespace d20Web.Storage
{
    /// <summary>
    /// Interface for storing combat information
    /// </summary>
    public interface ICombatStorage
    {
        /// <summary>
        /// Begins combat in the given campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="name">Name of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat</returns>
        Task<string> BeginCombat(string campaignID, string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates a given combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combat">Combat information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombat(string campaignID, Combat combat, CancellationToken cancellationToken = default);
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
        /// <returns>ID of the campaign, as well as the IDs of the combatants</returns>
        Task<(string, IEnumerable<string>)> CreateCombatants(string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the details of a combatant
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatant</param>
        /// <param name="combatant">Combatant information to use to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatant(string campaignID, string combatID, Combatant combatant, CancellationToken cancellationToken = default);
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
    }
}