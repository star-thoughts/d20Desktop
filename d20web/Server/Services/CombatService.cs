using d20Web.Hubs;
using d20Web.Models;
using d20Web.Models.Combat;
using d20Web.Storage;
using Microsoft.AspNetCore.SignalR;

namespace d20Web.Services
{
    /// <summary>
    /// Manages combat within the system
    /// </summary>
    public class CombatService : ICombatService
    {
        public CombatService(ILogger<CombatService> logger, ICombatStorage storage, IHubContext<CampaignHub> hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        private readonly IHubContext<CampaignHub> _hub;
        private readonly ILogger<CombatService> _logger;
        private readonly ICombatStorage _storage;

        #region Helpers
        private void ThrowIfInvalidCampaign(string campaignID)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
        }
        #endregion
        #region Combat Prep
        /// <summary>
        /// Creates a new combat prep in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create the combat in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat that was created</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        public async Task<string> CreateCombatPrep(string campaignID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);

            //  For now, only supports one active combat at a time, delete any existing ones
            foreach (CombatListData existing in await _storage.GetCombatPreparers(campaignID, cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(existing.ID))
                    await EndCombatPrep(campaignID, existing.ID, cancellationToken);
            }

            string id = await _storage.CreateCombatPrep(campaignID, cancellationToken);

            _ = _hub.CombatPrepCreated(campaignID, id);

            return id;
        }
        /// <summary>
        /// Gets information for a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is to be created in</param>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        public async Task<CombatPrep> GetCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            return await _storage.GetCombatPrep(campaignID, combatID, cancellationToken);
        }
        /// <summary>
        /// Gets a list of combat preps int he campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        public async Task<IEnumerable<CombatListData>> GetCombatPreps(string campaignID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));

            return await _storage.GetCombatPreparers(campaignID, cancellationToken);
        }
        /// <summary>
        /// Ends a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            await _storage.EndCombatPrep(campaignID, combatID, cancellationToken);

            _ = _hub.CombatPrepDeleted(campaignID, combatID);

        }
        /// <summary>
        /// Creates a combatant prep with the given statistics
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        public async Task<IEnumerable<string>> AddCombatantPreparers(string campaignID, string combatID, IEnumerable<CombatantPreparer> combatants, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (combatants == null)
                throw new ArgumentNullException(nameof(combatants));
            if (!combatants.Any())
                throw new ArgumentException(nameof(combatants));

            foreach (CombatantPreparer combatant in combatants)
            {
                if (string.IsNullOrWhiteSpace(combatant.Name))
                    throw new ArgumentNullException(nameof(combatant.Name));
            }

            IEnumerable<string> ids = await _storage.CreateCombatantPreparers(campaignID, combatID, combatants, cancellationToken);

            _ = _hub.CombatantPrepCreated(campaignID, combatID, ids);

            return ids;
        }
        /// <summary>
        /// Updates the stats for a combatant prep
        /// </summary>
        /// <param name="campaignID">Campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatant">Combatant details to use for the update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatantPreparer(string campaignID, string combatID, CombatantPreparer combatant, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (combatant == null)
                throw new ArgumentNullException(nameof(combatant));

            await _storage.UpdateCombatantPreparer(campaignID, combatID, combatant, cancellationToken);

            if (combatant.IsPlayer)
                _ = _hub.CombatantPrepUpdated(campaignID, combatID, combatant);

        }
        /// <summary>
        /// Deletes the combatant from the combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteCombatantPreparers(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            await _storage.DeleteCombatantPreparers(campaignID, combatID, combatantIDs, cancellationToken);

            _ = _hub.CombatantPrepsDeleted(campaignID, combatID, combatantIDs);
        }

        /// <summary>
        /// Gets the combatant preparers for a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combatants requested</returns>
        public async Task<IEnumerable<CombatantPreparer>> GetCombatantPreparers(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            return await _storage.GetCombatantPreparers(campaignID, combatID, combatantIDs, cancellationToken);
        }
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
        public async Task<string> CreateCombat(string campaignID, string name, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            //  For now, only supports one active combat at a time, delete any existing ones
            foreach (CombatListData existing in await _storage.GetCombats(campaignID, cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(existing.ID))
                    await EndCombat(campaignID, existing.ID, cancellationToken);
            }

            string id = await _storage.BeginCombat(campaignID, name, cancellationToken);

            _ = _hub.CombatCreated(campaignID, id, name);

            return id;
        }

        /// <summary>
        /// Ends a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombat(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            await _storage.EndCombat(campaignID, combatID, cancellationToken);

            _ = _hub.CombatDeleted(campaignID, combatID);
        }

        /// <summary>
        /// Gets a list of combats int he campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        public async Task<IEnumerable<CombatListData>> GetCombats(string campaignID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);

            return await _storage.GetCombats(campaignID, cancellationToken);
        }

        /// <summary>
        /// Updates the information for a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combat">Combat information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombat(string campaignID, Combat combat, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (combat == null)
                throw new ArgumentNullException(nameof(combat));

            await _storage.UpdateCombat(campaignID, combat, cancellationToken);

            _ = _hub.CombatUpdated(campaignID, combat);
        }

        /// <summary>
        /// Gets information for a combat
        /// </summary>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        public async Task<Combat> GetCombat(string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            return await _storage.GetCombat(combatID, cancellationToken);
        }
        /// <summary>
        /// Creates a combatant with the given statistics
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        public async Task<IEnumerable<string>> CreateCombatant(string campaignID, string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (combatants == null)
                throw new ArgumentNullException(nameof(combatants));
            if (!combatants.Any())
                throw new ArgumentException(nameof(combatants));

            foreach (Combatant combatant in combatants)
            {
                if (string.IsNullOrWhiteSpace(combatant.Name))
                    throw new ArgumentNullException(nameof(combatant.Name));
                if (combatant.Health == null)
                    throw new ArgumentNullException(nameof(combatant.Health));
            }

            IEnumerable<string> ids = await _storage.CreateCombatants(campaignID, combatID, combatants, cancellationToken);

            _ = _hub.CombatantCreated(campaignID, combatID, ids);

            return ids;
        }
        /// <summary>
        /// Updates the stats for a combatant
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatant">Combatant details to use for the update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatant(string campaignID, string combatID, Combatant combatant, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (combatant == null)
                throw new ArgumentNullException(nameof(combatant));

            await _storage.UpdateCombatant(campaignID, combatID, combatant, cancellationToken);

            _ = _hub.CombatantUpdated(campaignID, combatID, combatant.ToPlayerView());
        }
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<Combatant> GetCombatant(string combatID, string combatantID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (string.IsNullOrWhiteSpace(combatantID))
                throw new ArgumentNullException(nameof(combatantID));

            return await _storage.GetCombatant(combatID, combatantID, cancellationToken);
        }
        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<IEnumerable<Combatant>> GetCombatants(string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            IEnumerable<Combatant> result = await _storage.GetCombatants(combatID, cancellationToken);

            //  For now, we assume all clients are players and return only what they have access to
            result = result.Where(p => p.CanDisplayToPlayers())
                .Select(p => p.ToPlayerView());

            return result;
        }
        /// <summary>
        /// Deletes the combatant from the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatants to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteCombatant(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            await _storage.DeleteCombatant(campaignID, combatID, combatantIDs, cancellationToken);

            _ = _hub.CombatantsDeleted(campaignID, combatID, combatantIDs);
        }
        #endregion
        #region Scenarios
        /// <summary>
        /// Creates a combat scenario to allow for creating combats
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the created scenario</returns>
        public async Task<string> CreateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (scenario == null)
                throw new ArgumentNullException(nameof(scenario));
            if (string.IsNullOrEmpty(scenario.Name))
                throw new ArgumentNullException(nameof(scenario.Name));

            string id = await _storage.CreateCombatScenario(campaignID, scenario, cancellationToken);

            _ = _hub.CombatScenarioCreated(campaignID, id, scenario.Name);

            return id;
        }
        /// <summary>
        /// Update an existing scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (scenario == null)
                throw new ArgumentNullException(nameof(scenario));
            if (string.IsNullOrEmpty(scenario.Name))
                throw new ArgumentNullException(nameof(scenario.Name));

            await _storage.UpdateCombatScenario(campaignID, scenario, cancellationToken);

            _ = _hub.CombatScenarioUpdated(campaignID, scenario);
        }
        /// <summary>
        /// Gets a scenario for creating a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat scenario information</returns>
        public async Task<CombatScenario> GetCombatScenario(string campaignID, string scenarioID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidScenarioParameters(campaignID, scenarioID);

            CombatScenario result = await _storage.GetCombatScenario(campaignID, scenarioID, cancellationToken);

            return result;
        }
        /// <summary>
        /// Deletes the given combat scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteCombatScenario(string campaignID, string scenarioID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidScenarioParameters(campaignID, scenarioID);

            await _storage.DeleteCombatScenario(campaignID, scenarioID, cancellationToken);

            _ = _hub.CombatScenarioDeleted(campaignID, scenarioID);
        }

        private void ThrowIfInvalidScenarioParameters(string campaignID, string scenarioID)
        {
            ThrowIfInvalidCampaign(campaignID);
            if (string.IsNullOrWhiteSpace(scenarioID))
                throw new ArgumentNullException(nameof(scenarioID));
        }

        /// <summary>
        /// Adds a combatant to a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to add to</param>
        /// <param name="template">Combatant information to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant added</returns>
        public async Task<string> AddScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidScenarioParameters(campaignID, scenarioID);

            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (string.IsNullOrWhiteSpace(template.Name))
                throw new ArgumentNullException(nameof(template.Name));

            string id = await _storage.AddScenarioCombatant(campaignID, scenarioID, template, cancellationToken);

            _ = _hub.ScenarioCombatantCreated(campaignID, scenarioID, template.Name);

            return id;
        }
        /// <summary>
        /// Updates a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to update a combatant in</param>
        /// <param name="template">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task<string?> UpdateScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidScenarioParameters(campaignID, scenarioID);

            if (template == null)
                throw new ArgumentNullException(nameof(template));

            string? id = await _storage.UpdateScenarioCombatant(campaignID, scenarioID, template, cancellationToken);

            _ = _hub.ScenarioCombatantUpdated(campaignID, template);

            return id;
        }
        /// <summary>
        /// Gets information for a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario containing the combatant</param>
        /// <param name="templateID">ID of the combatant to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<CombatantTemplate> GetScenarioCombatant(string campaignID, string scenarioID, string templateID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidScenarioParameters(campaignID, scenarioID);

            if (string.IsNullOrWhiteSpace(templateID))
                throw new ArgumentNullException(nameof(templateID));

            return await _storage.GetScenarioCombatant(campaignID, scenarioID, templateID, cancellationToken);
        }
        /// <summary>
        /// Deletes a combatant from a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario containing the combatant</param>
        /// <param name="templateID">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteScenarioCombatant(string campaignID, string scenarioID, string templateID, CancellationToken cancellationToken = default)
        {
            ThrowIfInvalidScenarioParameters(campaignID, scenarioID);

            if (string.IsNullOrWhiteSpace(templateID))
                throw new ArgumentNullException(nameof(templateID));

            await _storage.DeleteScenarioCombatant(campaignID, scenarioID, templateID, cancellationToken);

            _ = _hub.ScenarioCombatantDeleted(campaignID, templateID);
        }
        #endregion
    }
}
