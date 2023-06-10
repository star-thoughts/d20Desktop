using Fiction.GameScreen.Combat;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Class to handle communications with the combat server
    /// </summary>
    public sealed class CombatManagement : ICombatManagement
    {
        /// <summary>
        /// Constructs a new <see cref="CombatManagement"/> class
        /// </summary>
        /// <param name="client">HTTP Client to use for communicating with the server</param>
        public CombatManagement(HttpClient client)
        {
            _client = client;
        }

        private HttpClient _client;

        #region Combat Prep
        /// <summary>
        /// Begins combat preparation and gets the ID of the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is taking place in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat created</returns>
        public async Task<string?> BeginPrep(string campaignID, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/prep";

            using (StringContent content = new StringContent(string.Empty))
            {
                using (HttpResponseMessage response = await _client.PostAsync(uri, content))
                {
                    response.EnsureSuccessStatusCode();
                    return JsonSerializer.Deserialize<NewCombat>(await response.Content.ReadAsStringAsync(cancellationToken))?.combatID;
                }
            }
        }
        /// <summary>
        /// Ends the given combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/prep/{HttpUtility.UrlEncode(combatID)}";

            using (HttpResponseMessage result = await _client.DeleteAsync(uri, cancellationToken))
                result.EnsureSuccessStatusCode();

        }
        /// <summary>
        /// Adds the given combatants to a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combatants</param>
        /// <param name="combatID">ID of the combat to put the combatants in</param>
        /// <param name="combatants">Information about the combatants to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>IDs of the combatants added, in order they were put in <paramref name="combatants"/></returns>
        public async Task<IEnumerable<string>> AddCombatantPreparers(string campaignID, string combatID, IEnumerable<d20Web.Models.Combat.CombatantPreparer> combatants, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/prep/{HttpUtility.UrlEncode(combatID)}/combatant";

            using (HttpResponseMessage result = await _client.PostAsJsonAsync(uri, combatants, cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                NewCombatants? combatantInfo = JsonSerializer.Deserialize<NewCombatants>(await result.Content.ReadAsStringAsync());

                return combatantInfo?.combatantIDs ?? Enumerable.Empty<string>();
            }
        }
        /// <summary>
        /// Removes combatants from a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="IDs">IDs of the combatants to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task RemoveCombatantPreparers(string campaignID, string combatID, IEnumerable<string> IDs, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/prep/{HttpUtility.UrlEncode(combatID)}/combatant";
            uri = QueryHelpers.AddQueryString(uri, IDs.Select(p => new KeyValuePair<string, string>("combatantIDs", p)));

            using (HttpResponseMessage result = await _client.DeleteAsync(uri, cancellationToken))
                result.EnsureSuccessStatusCode();

        }
        /// <summary>
        /// Updates the combatant information on the server
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="combatant">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatantPreparer(string campaignID, string combatID, d20Web.Models.Combat.CombatantPreparer combatant, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/prep/{HttpUtility.UrlEncode(combatID)}/combatant/{HttpUtility.UrlEncode(combatant.ID)}";

            using (HttpResponseMessage result = await _client.PutAsJsonAsync(uri, combatant, cancellationToken))
                result.EnsureSuccessStatusCode();
        }
        #endregion
        #region Combat
        /// <summary>
        /// Begins combat and gets the ID of the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is taking place in</param>
        /// <param name="combatName">Name to give this combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat created</returns>
        public async Task<string?> BeginCombat(string campaignID, string combatName, CancellationToken cancellationToken = default)
        {
            string uri = QueryHelpers.AddQueryString($"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat", "name", combatName);

            using (StringContent content = new StringContent(string.Empty))
            {
                using (HttpResponseMessage response = await _client.PostAsync(uri, content))
                {
                    response.EnsureSuccessStatusCode();
                    return JsonSerializer.Deserialize<NewCombat>(await response.Content.ReadAsStringAsync(cancellationToken))?.combatID;
                }
            }
        }

        /// <summary>
        /// Ends the given combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombat(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}";

            using (HttpResponseMessage result = await _client.DeleteAsync(uri, cancellationToken))
                result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates the combat with the most recent data
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to update</param>
        /// <param name="cancellationToken">Combat to update</param>
        /// <param name="combat">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombat(string campaignID, string combatID, d20Web.Models.Combat.Combat combat, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}";

            using (HttpResponseMessage result = await _client.PutAsJsonAsync(uri, combat, cancellationToken))
            {
                result.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Adds the given combatants to a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combatants</param>
        /// <param name="combatID">ID of the combat to put the combatants in</param>
        /// <param name="combatants">Information about the combatants to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>IDs of the combatants added, in order they were put in <paramref name="combatants"/></returns>
        public async Task<IEnumerable<string>> AddCombatants(string campaignID, string combatID, IEnumerable<d20Web.Models.Combat.Combatant> combatants, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}/combatant";

            using (HttpResponseMessage result = await _client.PostAsJsonAsync(uri, combatants, cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                NewCombatants? combatantInfo = JsonSerializer.Deserialize<NewCombatants>(await result.Content.ReadAsStringAsync());

                return combatantInfo?.combatantIDs ?? Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Removes combatants from a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="IDs">IDs of the combatants to remove</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task RemoveCombatants(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}/combatant";
            uri = QueryHelpers.AddQueryString(uri, combatantIDs.Select(p => new KeyValuePair<string, string>("combatantIDs", p)));

            using (HttpResponseMessage result = await _client.DeleteAsync(uri, cancellationToken))
                result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates the combatant information on the server
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="combatant">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatant(string campaignID, string combatID, d20Web.Models.Combat.Combatant combatant, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}/combatant/{HttpUtility.UrlEncode(combatant.ID)}";

            using (HttpResponseMessage result = await _client.PutAsJsonAsync(uri, combatant, cancellationToken))
                result.EnsureSuccessStatusCode();
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
        public async Task CreateCombatScenario(string campaignID, Combat.CombatScenario scenario, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/scenario";

            using (HttpResponseMessage result = await _client.PostAsJsonAsync(uri, scenario.ToServerScenario(), cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                string id = await GetNewObjectID(result);
                scenario.ServerID = id;
            }
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
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/scenario";

            using (HttpResponseMessage result = await _client.PutAsJsonAsync(uri, scenario.ToServerScenario(), cancellationToken))
                result.EnsureSuccessStatusCode();
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
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/scenario/{HttpUtility.UrlEncode(scenarioID)}";

            using (HttpResponseMessage result = await _client.DeleteAsync(uri, cancellationToken))
                result.EnsureSuccessStatusCode();
        }
        /// <summary>
        /// Adds a combatant to a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to add to</param>
        /// <param name="template">Combatant information to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant added</returns>
        public async Task AddScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/scenario/{HttpUtility.UrlEncode(scenarioID)}/combatant";

            using (HttpResponseMessage response = await _client.PostAsJsonAsync(uri, template.ToServerTemplate(), cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                string id = await GetNewObjectID(response);
                template.ServerID = id;
            }
        }
        /// <summary>
        /// Updates a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to update a combatant in</param>
        /// <param name="template">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/scenario/{HttpUtility.UrlEncode(scenarioID)}/combatant";

            using (HttpResponseMessage response = await _client.PutAsJsonAsync(uri, template.ToServerTemplate(), cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                string id = await GetNewObjectID(response);
                template.ServerID = id;
            }
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
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/scenario/{HttpUtility.UrlEncode(scenarioID)}/combatant/{HttpUtility.UrlEncode(templateID)}";

            using (HttpResponseMessage response = await _client.DeleteAsync(uri, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
            }
        }
        #endregion
        #region Helpers

        public void Dispose()
        {
            _client?.Dispose();
        }

        private async Task<string> GetNewObjectID(HttpResponseMessage result)
        {
            string json = await result.Content.ReadAsStringAsync();
            string id = JsonSerializer.Deserialize<NewObject>(json)?.id ?? string.Empty;
            return id;
        }

        class NewCombat
        {
            public string? combatID { get; set; }
        }

        class NewCombatants
        {
            public IEnumerable<string>? combatantIDs { get; set; }
        }
        class NewObject
        {
            public string? id { get; set; }
        }
        #endregion
    }
}
