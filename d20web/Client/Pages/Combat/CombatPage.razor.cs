using d20Web.Clients;
using d20Web.Models;
using d20Web.SignalRClient;
using Microsoft.AspNetCore.Components;

namespace d20Web.Pages.Combat
{
    /// <summary>
    /// Page for viewing combat
    /// </summary>
    public partial class CombatPage
    {
        [Inject]
        ICombatServer CombatServer { get; set; } = null!;
        [Inject]
        CombatClient CombatClient { get; set; } = null!;
        [Inject]
        NavigationManager NavigationManager { get; set; } = null!;

        [Parameter]
        public string? CampaignID { get; set; }
        [Parameter]
        public string? CombatID { get; set; }

        Models.Combat? Combat { get; set; }
        Combatant[]? Combatants { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(CampaignID)
                && !string.IsNullOrWhiteSpace(CombatID))
            {
                Combat = await CombatServer.GetCombat(CampaignID, CombatID);

                Combatants = OrderCombatants(await CombatServer.GetCombatants(CampaignID, CombatID));

                CombatClient.CombatDeleted += CombatClient_CombatDeleted;
                CombatClient.CombatantUpdated += CombatClient_CombatantUpdated;
                CombatClient.CombatantCreated += CombatClient_CombatantCreated;
                CombatClient.CombatantDeleted += CombatClient_CombatantDeleted;
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
                    foreach (Combatant combatant in combatants)
                        temp.Add(combatant);

                    Combatants = OrderCombatants(temp);
                }
                else
                    Combatants = OrderCombatants(combatants);
            }
        }

        private void AddOrUpdateCombatant(Combatant combatant)
        {
            if (Combatants != null)
            {
                List<Combatant> temp = Combatants.Where(p => !string.Equals(p.ID, combatant.ID, StringComparison.OrdinalIgnoreCase)).ToList();
                temp.Add(combatant);
                Combatants = OrderCombatants(temp);
            }
            else
                Combatants = new Combatant[] { combatant };
        }

        private void RemoveCombatant(IEnumerable<string> combatantIDs)
        {
            if (Combatants != null)
            {
                Combatants = OrderCombatants(Combatants.Where(p => combatantIDs.Contains(p.ID)));
            }
        }

        private void CombatClient_CombatantDeleted(object? sender, CombatantDeletedEventArgs e)
        {
            RemoveCombatant(e.CombatantIDs);
        }

        private async void CombatClient_CombatantCreated(object? sender, CombatantCreatedEventArgs e)
        {
            if (string.Equals(CombatID, e.CombatID, StringComparison.OrdinalIgnoreCase))
            {
                await AddOrUpdateCombatants(e.CombatantIDs);
            }
        }

        private void CombatClient_CombatantUpdated(object? sender, CombatantUpdatedEventArgs e)
        {
            if (string.Equals(CombatID, e.CombatID, StringComparison.OrdinalIgnoreCase))
            {
                AddOrUpdateCombatant(e.Combatant);
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
    }
}
