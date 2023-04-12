using d20Web.Clients;
using d20Web.Models;
using Microsoft.AspNetCore.Components;

namespace d20Web.Pages
{
    /// <summary>
    /// Default landing page
    /// </summary>
    public partial class Index
    {
        [Inject]
        public ICampaignServer CampaignServer { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        CampaignListData[]? Campaigns { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UpdateCampaignList(await CampaignServer.GetCampaigns());
            //  For now, automatically enter a campaign
            if (Campaigns?.Any() == true)
            {
                CampaignListData campaign = Campaigns[0];
                if (!string.IsNullOrWhiteSpace(campaign.ID))
                    NavigationManager.NavigateToCampaign(campaign.ID);
            }
        }

        void UpdateCampaignList(IEnumerable<CampaignListData> campaigns)
        {
            Campaigns = campaigns.OrderBy(p => p.Name).ToArray();
        }
    }
}
