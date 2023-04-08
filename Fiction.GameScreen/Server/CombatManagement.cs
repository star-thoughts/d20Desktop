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

            await _client.DeleteAsync(uri, cancellationToken);
        }

        /// <summary>
        /// Updates the combat with the most recent data
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to update</param>
        /// <param name="cancellationToken">Combat to update</param>
        /// <param name="combat">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombat(string campaignID, string combatID, d20Web.Models.Combat combat, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}";

            HttpResponseMessage result = await _client.PutAsJsonAsync(uri, combat, cancellationToken);
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
        public async Task<IEnumerable<string>> AddCombatants(string campaignID, string combatID, IEnumerable<d20Web.Models.Combatant> combatants, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}";

            HttpResponseMessage result = await _client.PostAsJsonAsync(uri, combatants, cancellationToken);
            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<NewCombatants>(await result.Content.ReadAsStringAsync())?.combatantIDs ?? Enumerable.Empty<string>();
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

            await _client.DeleteAsync(uri, cancellationToken);
        }

        /// <summary>
        /// Updates the combatant information on the server
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatants</param>
        /// <param name="combatant">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatant(string campaignID, string combatID, d20Web.Models.Combatant combatant, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}/combatant/{HttpUtility.UrlEncode(combatant.ID)}";

            await _client.PutAsJsonAsync(uri, combatant, cancellationToken);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        class NewCombat
        {
            public string? combatID { get; set; }
        }

        class NewCombatants
        {
            public IEnumerable<string>? combatantIDs { get; set; }
        }
    }
}
