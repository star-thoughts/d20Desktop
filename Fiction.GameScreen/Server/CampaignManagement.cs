using d20Web.Models;
using Fiction.GameScreen.Players;
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
        #region Campaigns
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
        #endregion
        #region Monsters
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
        #endregion
        #region Players
        /// <summary>
        /// Creates a new player character
        /// </summary>
        /// <param name="playerCharacter">Character data to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the player character created</returns>
        public async Task CreatePlayerCharacter(PlayerCharacter playerCharacter, CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign/{_campaignID}/players/character";

            using (HttpResponseMessage result = await _client.PostAsJsonAsync(uri, playerCharacter.ToServerCharacter(), cancellationToken))
            {
                result.EnsureSuccessStatusCode();

                string json = await result.Content.ReadAsStringAsync();
                string id = JsonSerializer.Deserialize<NewObject>(json)?.id ?? string.Empty;
                playerCharacter.ServerID = id;
            }
        }
        /// <summary>
        /// Gets a collection of all player characters in the campaign
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of player characters</returns>
        public async Task<PlayerCharacter[]> GetPlayerCharacters(CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign/{_campaignID}/players/character";

            using (HttpResponseMessage result = await _client.GetAsync(uri, cancellationToken))
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Gets the data for a player character
        /// </summary>
        /// <param name="id">ID of the character to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Player character data</returns>
        public Task<PlayerCharacter> GetPlayerCharacter(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the given player character
        /// </summary>
        /// <param name="id">ID of the character to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchonrous completion</returns>
        public async Task DeletePlayerCharacter(string id, CancellationToken cancellationToken = default)
        {
            string uri = $"api/campaign/{_campaignID}/players/character/{id}";

            using (HttpResponseMessage result = await _client.DeleteAsync(uri, cancellationToken))
            {
                result.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Updates the data for a player character
        /// </summary>
        /// <param name="character">Character data to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdatePlayerCharacter(PlayerCharacter character, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(character.ServerID))
                throw new ArgumentNullException(nameof(character.ServerID));

            string uri = $"api/campaign/{_campaignID}/players/character";

            using (HttpResponseMessage result = await _client.PutAsJsonAsync(uri, character.ToServerCharacter(), cancellationToken))
            {
                result.EnsureSuccessStatusCode();
            }
        }

        #endregion
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
