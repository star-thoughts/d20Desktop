using d20Web.Clients;
using d20Web.Models.Combat;
using d20Web.SignalRClient;
using Microsoft.AspNetCore.Components;

namespace d20Web.Pages.Combat.Prep
{
    public partial class CombatPrepPage
    {
        [Inject]
        public ICombatServer CombatServer { get; set; } = null!;
        [Inject]
        public CombatClient CombatClient { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public string? CampaignID { get; set; }
        [Parameter]
        public string? CombatID { get; set; }

        CombatPrep? Combat { get; set; }
        CombatantPreparer[]? Combatants { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID)
                && !string.IsNullOrWhiteSpace(CombatID))
            {
                Combat = await CombatServer.GetCombatPrep(CampaignID, CombatID);

                if (Combat != null)
                {
                    await CombatClient.StartClient();
                    CombatClient.CombatPrepDeleted += CombatClient_CombatDeleted;
                    CombatClient.CombatantPrepUpdated += CombatClient_CombatantUpdated;
                    CombatClient.CombatantPrepCreated += CombatClient_CombatantCreated;
                    CombatClient.CombatantPrepDeleted += CombatClient_CombatantDeleted;

                    Combatants = OrderCombatants(Combat.Combatants ?? await CombatServer.GetCombatantPreparers(CampaignID, CombatID));
                }
            }
        }

        private async Task AddOrUpdateCombatants(IEnumerable<string> combatantIDs)
        {
            if (!string.IsNullOrWhiteSpace(CampaignID)
                && !string.IsNullOrWhiteSpace(CombatID))
            {
                IEnumerable<CombatantPreparer> combatants = await CombatServer.GetCombatantPreparers(CampaignID, CombatID);

                if (Combatants != null)
                {
                    List<CombatantPreparer> temp = Combatants.Where(p => !combatantIDs.Contains(p.ID)).ToList();
                    foreach (CombatantPreparer combatant in combatants.Where(p => combatantIDs.Contains(p.ID)))
                        temp.Add(combatant);

                    Combatants = OrderCombatants(temp);
                }
                else
                    Combatants = OrderCombatants(combatants);

                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task AddOrUpdateCombatant(CombatantPreparer combatant)
        {
            if (Combatants != null)
            {
                List<CombatantPreparer> temp = Combatants.Where(p => !string.Equals(p.ID, combatant.ID, StringComparison.OrdinalIgnoreCase)).ToList();
                temp.Add(combatant);
                Combatants = OrderCombatants(temp);
            }
            else
                Combatants = new CombatantPreparer[] { combatant };

            await InvokeAsync(StateHasChanged);
        }

        private async Task RemoveCombatant(IEnumerable<string> combatantIDs)
        {
            if (Combatants != null)
            {
                Combatants = OrderCombatants(Combatants.Where(p => combatantIDs.Contains(p.ID)));
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void CombatClient_CombatantDeleted(object? sender, CombatantDeletedEventArgs e)
        {
            await RemoveCombatant(e.CombatantIDs);
        }

        private async void CombatClient_CombatantCreated(object? sender, CombatantCreatedEventArgs e)
        {
            if (string.Equals(CombatID, e.CombatID, StringComparison.OrdinalIgnoreCase))
            {
                await AddOrUpdateCombatants(e.CombatantIDs);
            }
        }

        private async void CombatClient_CombatantUpdated(object? sender, CombatantPrepUpdatedEventArgs e)
        {
            if (string.Equals(CombatID, e.CombatID, StringComparison.OrdinalIgnoreCase))
            {
                await AddOrUpdateCombatant(e.Combatant);
            }
        }

        private void CombatClient_CombatDeleted(object? sender, CombatDeletedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CampaignID))
                NavigationManager.NavigateToCampaign(CampaignID);
        }

        private CombatantPreparer[] OrderCombatants(IEnumerable<CombatantPreparer> enumerable)
        {
            enumerable ??= Enumerable.Empty<CombatantPreparer>();

            return enumerable.OrderBy(p => p.Name).ToArray();
        }

        private string CombatantClass(CombatantPreparer combatant)
        {
            return "";
        }


        private bool CanViewCombatant(CombatantPreparer combatant)
        {
            return combatant.IsPlayer;
        }
        public void Dispose()
        {
            CombatClient.CombatPrepDeleted -= CombatClient_CombatDeleted;
            CombatClient.CombatantPrepUpdated -= CombatClient_CombatantUpdated;
            CombatClient.CombatantPrepCreated -= CombatClient_CombatantCreated;
            CombatClient.CombatantPrepDeleted -= CombatClient_CombatantDeleted;
        }
        string GetInitString(CombatantPreparer combatant)
        {
            return $"{combatant.InitiativeRoll + combatant.InitiativeModifier} = {combatant.InitiativeRoll} + {combatant.InitiativeModifier}";
        }
    }
}
