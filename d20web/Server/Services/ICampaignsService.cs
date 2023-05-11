using d20Web.Models;
using d20Web.Models.Bestiary;

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
    }
}