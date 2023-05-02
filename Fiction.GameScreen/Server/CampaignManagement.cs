using d20Web.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Handles communicating with the server for campaign management
    /// </summary>
    public sealed class CampaignManagement : ICampaignManagement
    {
        public CampaignManagement(HttpClient client)
        {
            _client = client;
            Combat = new CombatManagement(client);
        }

        private HttpClient _client;

        /// <summary>
        /// Gets the interface to use for combat management
        /// </summary>
        public ICombatManagement Combat { get; }

        /// <summary>
        /// Requests a campaign be created on the server
        /// </summary>
        /// <param name="name">Name of the campaign to create</param>
        /// <returns>ID of the campaign that was created</returns>
        public async Task<string> CreateCampaign(string name)
        {
            string uri = QueryHelpers.AddQueryString("api/campaign", "name", name);

            using (StringContent content = new StringContent(string.Empty))
            {
                using (HttpResponseMessage result = await _client.PostAsync(uri, content))
                {
                    result.EnsureSuccessStatusCode();

                    string json = await result.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<NewCampaign>(json)?.campaignID ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets a list of campaigns from the server
        /// </summary>
        /// <returns>List of campaigns from the server</returns>
        public async Task<IEnumerable<CampaignListData>> GetCampaigns()
        {
            string uri = "api/campaign";

            using (HttpResponseMessage result = await _client.GetAsync(uri))
            {
                result.EnsureSuccessStatusCode();

                string json = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<CampaignListData>>(json) ?? Enumerable.Empty<CampaignListData>();
            }
        }

        class NewCampaign
        {
            public string? campaignID { get; set; }
        }
    }
}
