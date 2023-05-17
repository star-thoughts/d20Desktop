using d20Web.Clients;
using d20Web.Models.Combat;
using d20Web.Services;
using d20Web.SignalRClient;
using Microsoft.AspNetCore.Components;

namespace d20Web.Pages.Combat
{
    /// <summary>
    /// Page for viewing combat
    /// </summary>
    public partial class CombatPage : IDisposable
    {
        [Inject]
        public ICombatServer CombatServer { get; set; } = null!;
        [Inject]
        public CombatClient CombatClient { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public ViewService ViewService { get; set; } = null!;
        [Parameter]
        public string? CampaignID { get; set; }
        [Parameter]
        public string? CombatID { get; set; }

        Models.Combat.Combat? Combat { get; set; }
        Combatant[]? Combatants { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID)
                && !string.IsNullOrWhiteSpace(CombatID))
            {
                Combat = await CombatServer.GetCombat(CampaignID, CombatID);

                await CombatClient.StartClient();
                CombatClient.CombatDeleted += CombatClient_CombatDeleted;
                CombatClient.CombatUpdated += CombatClient_CombatUpdated;
                CombatClient.CombatantUpdated += CombatClient_CombatantUpdated;
                CombatClient.CombatantCreated += CombatClient_CombatantCreated;
                CombatClient.CombatantDeleted += CombatClient_CombatantDeleted;

                Combatants = OrderCombatants(await CombatServer.GetCombatants(CampaignID, CombatID));
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            string? id = Combatants?.FirstOrDefault(p => p.IsCurrent)?.ID;

            if (!string.IsNullOrWhiteSpace(id))
                await ViewService.ScrollIntoView(id);
        }

        private async void CombatClient_CombatUpdated(object? sender, CombatUpdatedEventArgs e)
        {
            if (string.Equals(e.Combat?.ID, Combat?.ID, StringComparison.OrdinalIgnoreCase))
            {
                Combat = e.Combat;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task AddOrUpdateCombatants(IEnumerable<string> combatantIDs)
        {
            if (!string.IsNullOrWhiteSpace(CampaignID)
                && !string.IsNullOrWhiteSpace(CombatID))
            {
                IEnumerable<Combatant> combatants = await CombatServer.GetCombatants(CampaignID, CombatID);

                if (Combatants != null)
                {
                    List<Combatant> temp = Combatants.Where(p => !combatantIDs.Contains(p.ID)).ToList();
                    foreach (Combatant combatant in combatants.Where(p => combatantIDs.Contains(p.ID)))
                        temp.Add(combatant);

                    Combatants = OrderCombatants(temp);
                }
                else
                    Combatants = OrderCombatants(combatants);

                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task AddOrUpdateCombatant(Combatant combatant)
        {
            if (Combatants != null)
            {
                List<Combatant> temp = Combatants.Where(p => !string.Equals(p.ID, combatant.ID, StringComparison.OrdinalIgnoreCase)).ToList();
                temp.Add(combatant);
                Combatants = OrderCombatants(temp);
            }
            else
                Combatants = new Combatant[] { combatant };

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

        private async void CombatClient_CombatantUpdated(object? sender, CombatantUpdatedEventArgs e)
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

        private Combatant[] OrderCombatants(IEnumerable<Combatant> enumerable)
        {
            enumerable ??= Enumerable.Empty<Combatant>();

            return enumerable.OrderBy(p => p.InitiativeOrder).ToArray();
        }

        private string CombatantClass(Combatant combatant)
        {
            return combatant.IsCurrent ? "current-combatant" : "";
        }


        private bool CanViewCombatant(Combatant combatant)
        {
            if (combatant.IsPlayer)
                return true;

            return combatant.HasGoneOnce && combatant.IncludeInCombat && combatant.DisplayToPlayers;
        }
        public void Dispose()
        {
            CombatClient.CombatUpdated -= CombatClient_CombatUpdated;
            CombatClient.CombatDeleted -= CombatClient_CombatDeleted;
            CombatClient.CombatantUpdated -= CombatClient_CombatantUpdated;
            CombatClient.CombatantCreated -= CombatClient_CombatantCreated;
            CombatClient.CombatantDeleted -= CombatClient_CombatantDeleted;
        }
    }
}
