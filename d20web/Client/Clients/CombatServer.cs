using d20Web.Models;
using System.Text.Json;
using System.Web;

namespace d20Web.Clients
{
    /// <summary>
    /// Client to communicate with a combat server
    /// </summary>
    public sealed class CombatServer : ICombatServer
    {
        /// <summary>
        /// Constructs a new <see cref="CombatServer"/>
        /// </summary>
        /// <param name="client">HTTP client for communicating with the server</param>
        public CombatServer(HttpClient client)
        {
            _client = client;
        }

        private HttpClient _client;

        /// <summary>
        /// Gets a list of combats in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        public async Task<IEnumerable<CombatListData>> GetCombats(string campaignID, CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat";

            using (HttpResponseMessage result = await _client.GetAsync(uri, cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<IEnumerable<CombatListData>>(await result.Content.ReadAsStringAsync(cancellationToken))
                    ?? Enumerable.Empty<CombatListData>();
            }
        }

        /// <summary>
        /// Gets combat information
        /// </summary>
        /// <param name="campaignID">ID of the campaign that contains the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        public async Task<Combat> GetCombat(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}";

            using (HttpResponseMessage result = await _client.GetAsync(uri, cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<Combat>(await result.Content.ReadAsStringAsync(cancellationToken))
                    ?? new Combat();
            }
        }
        /// <summary>
        /// Gets details about a combat's combatants
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat's combatants to retrieve</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<IEnumerable<Combatant>> GetCombatants(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign/{HttpUtility.UrlEncode(campaignID)}/combat/{HttpUtility.UrlEncode(combatID)}/combatant";

            using (HttpResponseMessage result = await _client.GetAsync(uri, cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<IEnumerable<Combatant>>(await result.Content.ReadAsStringAsync(cancellationToken))
                    ?? Enumerable.Empty<Combatant>();
            }
        }
    }
}
