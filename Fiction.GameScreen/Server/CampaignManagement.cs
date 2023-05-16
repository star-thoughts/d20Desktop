using d20Web.Models;
using d20Web.Models.Bestiary;
using Fiction.GameScreen.Monsters;
using System.Net.Http.Json;
using System.Text.Json;

namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Handles communicating with the server for campaign management
    /// </summary>
    public sealed class CampaignManagement : ICampaignManagement
    {
        public CampaignManagement(HttpClient client, string? campaignID = null)
        {
            _client = client;
            Combat = new CombatManagement(client);
            _campaignID = campaignID;
        }

        private HttpClient _client;
        private string? _campaignID;

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
                    _campaignID = JsonSerializer.Deserialize<NewCampaign>(json)?.campaignID ?? string.Empty;
                    return _campaignID;
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

        /// <summary>
        /// Creates a new monster
        /// </summary>
        /// <param name="monster">Monster information to create</param>
        /// <returns>Task for asyncrhonous completion</returns>
        public async Task CreateMonster(Monsters.Monster monster)
        {
            string uri = $"api/campaign/{_campaignID}/bestiary";

            d20Web.Models.Bestiary.Monster serverMonster = monster.ToServerMonster();
            
            using (HttpResponseMessage result = await _client.PostAsJsonAsync(uri, serverMonster))
            {
                result.EnsureSuccessStatusCode();

                string json = await result.Content.ReadAsStringAsync();
                string id = JsonSerializer.Deserialize<NewObject>(json)?.id ?? string.Empty;
                monster.ServerID = id;
            }
        }

        public async Task UpdateMonster(Monsters.Monster monster)
        {
            string uri = $"api/campaign/{_campaignID}/bestiary/{monster.ServerID}";

            d20Web.Models.Bestiary.Monster serverMonster = monster.ToServerMonster();

            using (HttpResponseMessage result = await _client.PutAsJsonAsync(uri, serverMonster))
            {
                result.EnsureSuccessStatusCode();
            }
        }
        /// <summary>
        /// Requests a monster to be deleted from the current campaign
        /// </summary>
        /// <param name="monsterId">ID of the monster</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteMonster(string monsterId)
        {
            string uri = $"api/campaign/{_campaignID}/bestiary/{monsterId}";

            using (HttpResponseMessage result = await _client.DeleteAsync(uri))
            {
                result.EnsureSuccessStatusCode();
            }
        }

        class NewCampaign
        {
            public string? campaignID { get; set; }
        }

        class NewObject
        {
            public string? id { get; set; }
        }
    }
}
