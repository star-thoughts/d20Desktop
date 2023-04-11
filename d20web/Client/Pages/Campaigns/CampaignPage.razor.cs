using d20Web.Clients;
using d20Web.SignalRClient;
using Microsoft.AspNetCore.Components;

namespace d20Web.Pages.Campaigns
{
    /// <summary>
    /// Page for viewing base campaign information
    /// </summary>
    public partial class CampaignPage : IDisposable
    {
        [Inject]
        ICampaignServer CampaignServer { get; set; } = null!;
        [Inject]
        ICombatServer CombatServer { get; set; } = null!;
        [Inject]
        CombatClient CombatClient { get; set; } = null!;
        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;
        
        [Parameter]
        public string? CampaignID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID))
            {
                var combats = await CombatServer.GetCombats(CampaignID);
                var combat = combats?.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(combat?.ID))
                {
                    NavigationManager.NavigateToCombat(CampaignID, combat.ID);
                }
                else
                    ConnectToSignalR();
            }
        }

        private void ConnectToSignalR()
        {
            CombatClient.CombatStarted += CombatClient_CombatStarted;
        }

        private void CombatClient_CombatStarted(object? sender, CombatStartedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.CombatID)
                && string.Equals(CampaignID, e.CampaignID, StringComparison.OrdinalIgnoreCase))
            {
                NavigationManager.NavigateToCombat(e.CampaignID, e.CombatID);
            }
        }

        private void DisconnectSignalR()
        {
            CombatClient.CombatStarted -= CombatClient_CombatStarted;
        }

        public void Dispose()
        {
            DisconnectSignalR();
        }
    }
}
