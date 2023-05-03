using d20Web.Clients;
using d20Web.SignalRClient;
using Microsoft.AspNetCore.Components;
using System.Windows.Markup;

namespace d20Web.Pages.Campaigns
{
    /// <summary>
    /// Page for viewing base campaign information
    /// </summary>
    public partial class CampaignPage : IDisposable
    {
        [Inject]
        public ICampaignServer CampaignServer { get; set; } = null!;
        [Inject]
        public ICombatServer CombatServer { get; set; } = null!;
        [Inject]
        public CombatClient CombatClient { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public string? CampaignID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID))
            {
                await ConnectToSignalR();
                if (!await CheckForExistingCombat())
                    await CheckForExistingCombatPrep();
            }
        }

        private async Task<bool> CheckForExistingCombatPrep()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID))
            {
                IEnumerable<Models.CombatListData> combats = await CombatServer.GetCombatPreps(CampaignID);
                Models.CombatListData? combat = combats?.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(combat?.ID))
                {
                    NavigationManager.NavigateToCombatPrep(CampaignID, combat.ID);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> CheckForExistingCombat()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID))
            {
                IEnumerable<Models.CombatListData> combats = await CombatServer.GetCombats(CampaignID);
                Models.CombatListData? combat = combats?.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(combat?.ID))
                {
                    NavigationManager.NavigateToCombat(CampaignID, combat.ID);
                    return true;
                }
            }
            return false;
        }

        private async Task ConnectToSignalR()
        {
            await CombatClient.StartClient();
            CombatClient.CombatStarted += CombatClient_CombatStarted;
            CombatClient.CombatPrepStarted += CombatClient_CombatPrepStarted;
        }

        private void CombatClient_CombatPrepStarted(object? sender, CombatPrepStartedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.CombatID)
                && string.Equals(CampaignID, e.CampaignID, StringComparison.OrdinalIgnoreCase))
            {
                NavigationManager.NavigateToCombatPrep(e.CampaignID, e.CombatID);
            }
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
