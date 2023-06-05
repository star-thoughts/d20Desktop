using d20Web.Models;
using d20Web.Models.Bestiary;
using d20Web.Models.Players;

namespace d20Web.Services
{
    public interface ICampaignsService
    {
        Task<string> CreateCampaign(string campaignName, CancellationToken cancellationToken = default);
        Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default);
        Task<Campaign> GetCampaign(string campaignID, CancellationToken cancellationToken = default);
        Task<string> CreateMonster(string campaignID, Monster monster, CancellationToken cancellationToken = default);
        Task UpdateMonster(string campaignID, Monster monster, CancellationToken cancellationToken = default);
        Task DeleteMonster(string campaignID, string id, CancellationToken cancellationToken = default);
        Task<Monster> GetMonster(string campaignID, string id, CancellationToken requestAborted);
        Task<string> CreatePlayerCharacter(string campaignID, PlayerCharacter playerCharacter, CancellationToken cancellationToken = default);
        Task<PlayerCharacter[]> GetPlayerCharacters(string campaignID, CancellationToken cancellationToken = default);
        Task<PlayerCharacter> GetPlayerCharacter(string campaignID, string id, CancellationToken cancellationToken = default);
        Task DeletePlayerCharacter(string campaignID, string id, CancellationToken cancellationToken = default);
        Task UpdatePlayerCharacter(string campaignID, PlayerCharacter character, CancellationToken cancellationToken = default);
    }
}