using Fiction.GameScreen.Combat;

namespace Fiction.GameScreen.Server
{
    public interface ICombatManagement : IDisposable
    {
        #region Combat Prep
        /// <summary>
        /// Begins combat preparation and gets the ID of the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is taking place in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat created</returns>
        Task<string?> BeginPrep(string campaignID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ends the given combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task EndCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds the given combatants to a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combatants</param>
        /// <param name="combatID">ID of the combat to put the combatants in</param>
        /// <param name="combatants">Information about the combatants to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>IDs of the combatants added, in order they were put in <paramref name="combatants"/></returns>
        Task<IEnumerable<string>> AddCombatantPreparers(string campaignID, string combatID, IEnumerable<d20Web.Models.Combat.CombatantPreparer> combatants, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes combatants from a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="IDs">IDs of the combatants to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task RemoveCombatantPreparers(string campaignID, string combatID, IEnumerable<string> IDs, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the combatant information on the server
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="combatant">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatantPreparer(string campaignID, string combatID, d20Web.Models.Combat.CombatantPreparer combatant, CancellationToken cancellationToken = default);
        #endregion
        #region Combat
        /// <summary>
        /// Begins combat and gets the ID of the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is taking place in</param>
        /// <param name="combatName">Name to give this combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat created</returns>
        Task<string?> BeginCombat(string campaignID, string combatName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Ends the given combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task EndCombat(string campaignID, string combatID, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the combat with the most recent data
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to update</param>
        /// <param name="cancellationToken">Combat to update</param>
        /// <param name="combat">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombat(string campaignID, string combatID, d20Web.Models.Combat.Combat combat, CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds the given combatants to a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combatants</param>
        /// <param name="combatID">ID of the combat to put the combatants in</param>
        /// <param name="combatants">Information about the combatants to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>IDs of the combatants added, in order they were put in <paramref name="combatants"/></returns>
        Task<IEnumerable<string>> AddCombatants(string campaignID, string combatID, IEnumerable<d20Web.Models.Combat.Combatant> combatants, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes combatants from a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="IDs">IDs of the combatants to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task RemoveCombatants(string campaignID, string combatID, IEnumerable<string> IDs, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the combatant information on the server
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="combatant">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatant(string campaignID, string combatID, d20Web.Models.Combat.Combatant combatant, CancellationToken cancellationToken = default);
        #endregion
        #region Scenarios
        /// <summary>
        /// Creates a combat scenario to allow for creating combats
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the created scenario</returns>
        Task CreateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default);
        /// <summary>
        /// Update an existing scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default);
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
        Task AddScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to update a combatant in</param>
        /// <param name="template">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        Task UpdateScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default);
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
