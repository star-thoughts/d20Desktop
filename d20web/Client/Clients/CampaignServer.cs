using d20Web.Models;
using System.Text.Json;
using System.Web;

namespace d20Web.Clients
{
    /// <summary>
    /// Handles communications with the campaign server
    /// </summary>
    public class CampaignServer : ICampaignServer
    {
        /// <summary>
        /// Constructs a new <see cref="CampaignServer"/>
        /// </summary>
        /// <param name="client">HTTP client for communicating with the server</param>
        public CampaignServer(HttpClient client)
        {
            _client = client;
        }

        private HttpClient _client;

        /// <summary>
        /// Gets a list of combats in a campaign
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        public async Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign";

            using (var result = await _client.GetAsync(uri, cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<IEnumerable<CampaignListData>>(await result.Content.ReadAsStringAsync(cancellationToken))
                    ?? Enumerable.Empty<CampaignListData>();
            }
        }
    }
}
