using d20Web.Models.Combat;
using Fiction.GameScreen.Combat;
using System.ComponentModel;

namespace Fiction.GameScreen.Server
{
    /// <summary>
    /// Handles automated communications with combat server for an active combat
    /// </summary>
    public sealed class CombatPrepWatcher : IAsyncDisposable
    {
        /// <summary>
        /// Constructs a new <see cref="CombatPrepWatcher"/>
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat will take place in</param>
        /// <param name="combat">Combat preparation to watch</param>
        /// <param name="management">Interface for communicating with the combat server</param>
        public CombatPrepWatcher(string campaignID, CombatPreparer combat, ICombatManagement management)
        {
            _campaignID = campaignID;
            _combat = combat;
            _combatManagement = management;
            _combatantsMonitor = new CollectionMonitor(_combat.Combatants);

            _combat.PropertyChanged += _combat_PropertyChanged;
            _combatantsMonitor.PropertyChanged += _combatantsMonitor_PropertyChangedAsync;
            _combatantsMonitor.CollectionChanged += _combatantsMonitor_CollectionChangedAsync;
        }

        private string _campaignID;
        private CombatPreparer _combat;
        private ICombatManagement _combatManagement;
        private CollectionMonitor _combatantsMonitor;

        /// <summary>
        /// Initializes the watcher
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Whether or not this was initialized successfully</returns>
        public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(_combat.ServerID))
                {
                    string? id = await _combatManagement.BeginPrep(_campaignID, cancellationToken);
                    _combat.ServerID = id;

                    if (id != null)
                    {
                        string[] combatantIDs = (await _combatManagement.AddCombatantPreparers(_campaignID, id, _combat.Combatants.Select(p => p.ToServerCombatant()), cancellationToken)).ToArray();
                        for (int i = 0; i < combatantIDs.Length; i++)
                        {
                            Combat.CombatantPreparer combatant = _combat.Combatants[i];
                            combatant.ServerID = combatantIDs[i];
                        }
                    }
                }
                return !string.IsNullOrEmpty(_combat.ServerID);
            }
            catch
            {
                return false;
            }
        }

        private async void _combatantsMonitor_CollectionChangedAsync(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_combat.ServerID))
            {
                try
                {
                    d20Web.Models.Combat.CombatantPreparer[]? newCombatants = e.NewItems?.OfType<Combat.CombatantPreparer>().Select(p => p.ToServerCombatant()).ToArray();
                    string[]? oldCombatants = e.OldItems?.OfType<Combat.CombatantPreparer>()
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

        private async void _combatantsMonitor_PropertyChangedAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_combat.ServerID))
            {
                try
                {
                    if (sender is Combat.CombatantPreparer combatant
                        && !string.IsNullOrWhiteSpace(combatant.ServerID))
                    {
                        await _combatManagement.UpdateCombatantPreparer(_campaignID, _combat.ServerID, combatant.ToServerCombatant());
                    }
                }
                catch
                {
                }
            }
        }

        private void _combat_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

        private async Task RemoveCombatants(string[] combatantIDs)
        {
            if (!string.IsNullOrEmpty(_combat.ServerID))
            {
                await _combatManagement.RemoveCombatantPreparers(_campaignID, _combat.ServerID, combatantIDs);
            }
        }

        private async Task AddCombatants(d20Web.Models.Combat.CombatantPreparer[] newCombatants)
        {
            if (!string.IsNullOrEmpty(_combat.ServerID))
            {
                string[] ids = (await _combatManagement.AddCombatantPreparers(_campaignID, _combat.ServerID, newCombatants)).ToArray();

                if (ids.Length == newCombatants.Length)
                {
                    for (int i = 0; i < newCombatants.Length; i++)
                    {
                        d20Web.Models.Combat.CombatantPreparer combatant = newCombatants[i];
                        Combat.CombatantPreparer? oldCombatant = _combat.Combatants
                            .FirstOrDefault(p => string.Equals(p.Name, combatant.Name, StringComparison.Ordinal) && p.Ordinal == combatant.Ordinal);

                        if (oldCombatant != null)
                            oldCombatant.ServerID = ids[i];
                    }
                }
            }
        }

        /// <summary>
        /// Ends the preparer
        /// </summary>
        /// <returns>Task for asynchronous completion</returns>
        public async ValueTask DisposeAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_combat.ServerID))
                {
                    await _combatManagement.EndCombatPrep(_campaignID, _combat.ServerID);
                }
            }
            catch
            {
            }
        }
    }
}
