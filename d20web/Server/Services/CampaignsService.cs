using d20Web.Hubs;
using d20Web.Models;
using d20Web.Models.Bestiary;
using d20Web.Models.Players;
using d20Web.Storage;
using Microsoft.AspNetCore.SignalR;

namespace d20Web.Services
{
    public class CampaignsService : ICampaignsService
    {
        public CampaignsService(ICampaignStorage campaignStorage, IHubContext<CampaignHub> campaignHub, ILogger<CampaignsService> logger)
        {
            _campaignHub = campaignHub;
            _logger = logger;
            _campaignStorage = campaignStorage;
        }

        private IHubContext<CampaignHub> _campaignHub;
        private ICampaignStorage _campaignStorage;
        private ILogger _logger;

        #region Campaign
        public async Task<string> CreateCampaign(string campaignName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignName))
                throw new ArgumentNullException(nameof(campaignName));

            string id = await _campaignStorage.CreateCampaign(campaignName, cancellationToken);

            _ = _campaignHub.CampaignCreated(id);

            return id;
        }

        public async Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default)
        {
            return await _campaignStorage.GetCampaigns(cancellationToken);
        }

        public Task<Campaign> GetCampaign(string campaignID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));

            return _campaignStorage.GetCampaign(campaignID, cancellationToken);
        }
        #endregion
        #region Monsters
        public async Task<string> CreateMonster(string campaignID, Monster monster, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));

            string id = await _campaignStorage.CreateMonster(campaignID, monster, cancellationToken);

            _ = _campaignHub.MonsterCreated(campaignID, id);

            return id;
        }

        public async Task UpdateMonster(string campaignID, Monster monster, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));

            await _campaignStorage.UpdateMonster(campaignID, monster, cancellationToken);

            _ = _campaignHub.MonsterUpdated(campaignID, monster);
        }

        public async Task DeleteMonster(string campaignID, string id, CancellationToken cancellationToken = default)
        {
            await _campaignStorage.DeleteMonster(campaignID, id, cancellationToken);

            _ = _campaignHub.MonsterDeleted(campaignID, id);
        }

        public async Task<Monster> GetMonster(string campaignID, string id, CancellationToken cancellationToken = default)
        {
            return await _campaignStorage.GetMonster(campaignID, id, cancellationToken);
        }
        #endregion
        #region Players

        public async Task<string> CreatePlayerCharacter(string campaignID, PlayerCharacter playerCharacter, CancellationToken cancellationToken = default)
        {
            string id = await _campaignStorage.CreatePlayerCharacter(campaignID, playerCharacter, cancellationToken);

            _ = _campaignHub.PlayerCharacterCreated(campaignID, id);

            return id;
        }

        public async Task<PlayerCharacter[]> GetPlayerCharacters(string campaignID, CancellationToken cancellationToken = default)
        {
            return await _campaignStorage.GetPlayerCharacters(campaignID, cancellationToken);
        }

        public async Task<PlayerCharacter> GetPlayerCharacter(string campaignID, string id, CancellationToken cancellationToken = default)
        {
            return await _campaignStorage.GetPlayerCharacter(campaignID, id, cancellationToken);
        }

        public async Task DeletePlayerCharacter(string campaignID, string id, CancellationToken cancellationToken = default)
        {
            await _campaignStorage.DeletePlayerCharacter(campaignID, id, cancellationToken);

            _ = _campaignHub.PlayerCharacterDeleted(campaignID, id);
        }

        public async Task UpdatePlayerCharacter(string campaignID, PlayerCharacter character, CancellationToken cancellationToken = default)
        {
            await _campaignStorage.UpdatePlayerCharacter(campaignID, character, cancellationToken);

            _ = _campaignHub.PlayerCharacterUpdated(campaignID, character);
        }
        #endregion
    }
}
