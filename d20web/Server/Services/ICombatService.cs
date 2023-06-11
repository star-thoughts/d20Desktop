using d20Web.Models.Combat;

namespace d20Web.Services
{
    /// <summary>
    /// Interface for managing combat
    /// </summary>
    public interface ICombatService
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
        Task<IEnumerable<CombatListData>> GetCombatPreps(string campaignID, CancellationToken cancellationToken = default);
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
        Task<IEnumerable<string>> AddCombatantPreparers(string campaignID, string combatID, IEnumerable<CombatantPreparer> combatants, CancellationToken cancellationToken = default);

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
        /// Creates a new combat in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create the combat in</param>
        /// <param name="name">Name of the combat to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat that was created</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        Task<string> CreateCombat(string campaignID, string name, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ends a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task EndCombat(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the information for a combat
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
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        Task<IEnumerable<string>> CreateCombatant(string campaignID, string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        Task<Combatant> GetCombatant(string combatID, string combatantID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of combats that are currently running
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combats</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats</returns>
        Task<IEnumerable<CombatListData>> GetCombats(string campaignID, CancellationToken cancellationToken = default);
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
        /// <summary>
        /// Deletes the combatant from the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteCombatant(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default);
        #endregion
        #region Scenarios
        /// <summary>
        /// Creates a combat scenario to allow for creating combats
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the created scenario</returns>
        Task<string> CreateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update an existing scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a scenario for creating a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat scenario information</returns>
        Task<CombatScenario> GetCombatScenario(string campaignID, string scenarioID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the given combat scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteCombatScenario(string campaignID, string scenarioID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds a combatant to a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to add to</param>
        /// <param name="template">Combatant information to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant added</returns>
        Task<string> AddScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to update a combatant in</param>
        /// <param name="template">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task<string?> UpdateScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets information for a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario containing the combatant</param>
        /// <param name="templateID">ID of the combatant to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        Task<CombatantTemplate> GetScenarioCombatant(string campaignID, string scenarioID, string templateID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes a combatant from a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario containing the combatant</param>
        /// <param name="templateID">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task DeleteScenarioCombatant(string campaignID, string scenarioID, string templateID, CancellationToken cancellationToken = default);
        #endregion
    }
}