using d20Web.Models;

namespace d20Web.Services
{
    public interface ICampaignsService
    {
        Task<string> CreateCampaign(string campaignName, CancellationToken cancellationToken = default);
        Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default);
        Task<Campaign> GetCampaign(string campaignID, CancellationToken cancellationToken = default);
    }
}