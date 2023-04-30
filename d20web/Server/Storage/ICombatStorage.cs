using d20Web.Models;
using System.ComponentModel.DataAnnotations;

namespace d20Web.Storage
{
    /// <summary>
    /// Interface for storing combat information
    /// </summary>
    public interface ICombatStorage
    {
        #region Combat Prep
        /// <summary>
        /// Creates a new combat prep in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create the combat in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat that was created</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        Task<string> CreateCombatPrep(string campaignID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ends a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task EndCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a list of combat preps int he campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        Task<IEnumerable<CombatListData>> GetCombatPreparers(string campaignID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets information for a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create combat in</param>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        Task<CombatPrep> GetCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Creates a combatant prep with the given statistics
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        Task<IEnumerable<string>> CreateCombatantPreparers(string campaignID, string combatID, IEnumerable<CombatantPreparer> combatants, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the stats for a combatant prep
        /// </summary>
        /// <param name="campaignID">Campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatant">Combatant details to use for the update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatantPreparer(string campaignID, string combatID, CombatantPreparer combatant, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the combatant from the combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteCombatantPreparers(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the combatant preparers for a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combatants requested</returns>
        Task<IEnumerable<CombatantPreparer>> GetCombatantPreparers(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default);
        #endregion
        #region Combat
        /// <summary>
        /// Begins combat in the given campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="name">Name of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat</returns>
        Task<string> BeginCombat(string campaignID, string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ends a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task EndCombat(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a list of combats int he campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        Task<IEnumerable<CombatListData>> GetCombats(string campaignID, CancellationToken cancellationToken = default);
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
        /// Adds the given combatant to a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatants">Combatant information to add to the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the campaign, as well as the IDs of the newly created combatants</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null</exception>
        /// <exception cref="ArgumentException">One or more of the IDs could not be converted to a MongoDB ObjectID</exception>
        Task<IEnumerable<string>> CreateCombatants(string campaignID, string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default);
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
        /// <summary>
        /// Deletes the combatant from the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatants to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteCombatant(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default);
        #endregion
    }
}
