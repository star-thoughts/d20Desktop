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
            _combatantsMonitor.PropertyChanged += _combat_PropertyChanged;
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
                string? id = await _combatManagement.BeginCombat(_campaignID, _combat.Name, cancellationToken);
                _combat.ID = id;
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
                    await _combatManagement.UpdateCombat(_combat.ID, _combat.ToServerCombat());
                }
                catch
                {
                }
            }
        }

        private void _combatantsMonitor_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
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
    }
}
