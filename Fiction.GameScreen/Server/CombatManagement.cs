using Microsoft.AspNetCore.WebUtilities;
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
            string uri = QueryHelpers.AddQueryString("/api/combat", new Dictionary<string, string>
                {
                    { "campaignID", campaignID },
                    { "name", combatName },
                });

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
            string uri = QueryHelpers.AddQueryString($"/api/combat/{HttpUtility.UrlEncode(combatID)}", "campaignID", campaignID);

            await _client.DeleteAsync(uri, cancellationToken);
        }

        /// <summary>
        /// Updates the combat with the most recent data
        /// </summary>
        /// <param name="combatID">ID of the combat to update</param>
        /// <param name="cancellationToken">Combat to update</param>
        /// <param name="combat">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombat(string combatID, d20Web.Models.Combat combat, CancellationToken cancellationToken = default)
        {
            string uri = $"/api/combat/{HttpUtility.UrlEncode(combatID)}";

            await _client.PutAsJsonAsync(uri, combat, cancellationToken);
        }

        class NewCombat
        {
            public string? combatID { get; set; }
        }
    }
}
