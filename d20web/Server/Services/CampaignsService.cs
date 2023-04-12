using d20Web.Hubs;
using d20Web.Models;
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
    }
}
