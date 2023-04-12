using Fiction.GameScreen.Combat;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Handles automated communications with combat server for an active combat
    /// </summary>
    public sealed class CombatWatcher : IAsyncDisposable
    {
        /// <summary>
        /// Constructs a new <see cref="CombatWatcher"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat will take place in</param>
        /// <param name="combat">Combat to watch</param>
        /// <param name="management">Interface for communicating with the combat server</param>
        public CombatWatcher(string campaignID, ActiveCombat combat, ICombatManagement management)
        {
            _campaignID = campaignID;
            _combat = combat;
            _combatManagement = management;
            _combatantsMonitor = new CollectionMonitor(_combat.Combatants);

            _combat.PropertyChanged += _combat_PropertyChanged;
            _combatantsMonitor.PropertyChanged += _combatantsMonitor_PropertyChangedAsync; ;
            _combatantsMonitor.CollectionChanged += _combatantsMonitor_CollectionChanged;
        }

        private string _campaignID;
        private ActiveCombat _combat;
        private ICombatManagement _combatManagement;
        private CollectionMonitor _combatantsMonitor;

        /// <summary>
        /// Initializes the combat watcher
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Whether or not it initialized correctly.  If false, no further calls are needed.</returns>
        public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(_combat.ID))
                {
                    string? id = await _combatManagement.BeginCombat(_campaignID, _combat.Name, cancellationToken);
                    _combat.ID = id;

                    if (id != null)
                    {
                        string[] combatantIDs = (await _combatManagement.AddCombatants(_campaignID, id, _combat.Combatants.Select(p => p.ToServerCombatant()), cancellationToken)).ToArray();
                        for (int i = 0; i < combatantIDs.Length; i++)
                        {
                            ICombatant combatant = _combat.Combatants[i];
                            combatant.ServerID = combatantIDs[i];
                        }
                    }
                }
                return !string.IsNullOrEmpty(_combat.ID);
            }
            catch
            {
                return false;
            }
        }

        private async void _combat_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_combat.ID))
            {
                try
                {
                    await _combatManagement.UpdateCombat(_campaignID, _combat.ID, _combat.ToServerCombat());
                }
                catch
                {
                }
            }
        }

        private async void _combatantsMonitor_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_combat.ID))
            {
                try
                {
                    d20Web.Models.Combatant[]? newCombatants = e.NewItems?.OfType<ICombatant>().Select(p => p.ToServerCombatant()).ToArray();
                    string[]? oldCombatants = e.OldItems?.OfType<ICombatant>()
                        .Select(p => p.ServerID)
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .OfType<string>()
                        .ToArray();

                    if (newCombatants != null)
                        await AddCombatants(newCombatants);
                    if (oldCombatants != null)
                        await RemoveCombatants(oldCombatants);
                }
                catch
                {
                }
            }
        }

        private async Task RemoveCombatants(string[] combatantIDs)
        {
            if (!string.IsNullOrEmpty(_combat.ID))
            {
                await _combatManagement.RemoveCombatants(_campaignID, _combat.ID, combatantIDs);
            }
        }

        private async Task AddCombatants(d20Web.Models.Combatant[] newCombatants)
        {
            if (!string.IsNullOrEmpty(_combat.ID))
            {
                string[] ids = (await _combatManagement.AddCombatants(_campaignID, _combat.ID, newCombatants)).ToArray();

                if (ids.Length == newCombatants.Length)
                {
                    for (int i = 0; i < newCombatants.Length; i++)
                    {
                        d20Web.Models.Combatant combatant = newCombatants[i];
                        ICombatant? oldCombatant = _combat.Combatants
                            .FirstOrDefault(p => string.Equals(p.Name, combatant.Name, StringComparison.Ordinal) && p.Ordinal == combatant.Ordinal);

                        if (oldCombatant != null)
                            oldCombatant.ServerID = ids[i];
                    }
                }
            }
        }

        private async void _combatantsMonitor_PropertyChangedAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_combat.ID))
            {
                try
                {
                    if (sender is ICombatant combatant
                        && !string.IsNullOrWhiteSpace(combatant.ServerID))
                    {
                        await _combatManagement.UpdateCombatant(_campaignID, _combat.ID, combatant.ToServerCombatant());
                    }
                }
                catch
                {
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (!string.IsNullOrEmpty(_combat.ID))
            {
                try
                {
                    await _combatManagement.EndCombat(_campaignID, _combat.ID);
                }
                catch
                {
                }
            }
        }

        public async Task EndCombat(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(_combat.ID))
            {
                try
                {
                    await _combatManagement.EndCombat(_campaignID, _combat.ID, cancellationToken);
                }
                catch
                {
                }
            }
        }
    }
}
