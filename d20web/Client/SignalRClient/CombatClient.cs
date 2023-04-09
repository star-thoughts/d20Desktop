using d20Web.Hubs;
using d20Web.Models;

namespace d20Web.SignalRClient
{
    /// <summary>
    /// SignalR client for getting combat-related messages
    /// </summary>
    public class CombatClient : ClientBase
    {
        /// <summary>
        /// Constructs a new <see cref="CombatClient"/>
        /// </summary>
        /// <param name="baseUri">Base URI for connecting to the hub</param>
        /// <param name="loggingProvider">Logging provider</param>
        public CombatClient(Uri baseUri, ILoggerProvider loggingProvider)
            : base(new Uri($"{baseUri}hub/campaign"), loggingProvider)
        {
            AddMessageHandler<CombatStartedEventArgs, string, string, string>(Constants.CombatCreated, p => CombatStarted?.Invoke(this, p));
            AddMessageHandler<CombatDeletedEventArgs, string, string>(Constants.CombatCreated, p => CombatDeleted?.Invoke(this, p));
            AddMessageHandler<CombatUpdatedEventArgs, string, Combat>(Constants.CombatUpdated, p => CombatUpdated?.Invoke(this, p));
            AddMessageHandler<CombatantCreatedEventArgs, string, string, IEnumerable<string>>(Constants.CombatantCreated, p => CombatantCreated?.Invoke(this, p));
            AddMessageHandler<CombatantUpdatedEventArgs, string, string, Combatant>(Constants.CombatantUpdated, p => CombatantUpdated?.Invoke(this, p));
            AddMessageHandler<CombatantDeletedEventArgs, string, string, string>(Constants.CombatantsDeleted, p => CombatantDeleted?.Invoke(this, p));
        }

        public event EventHandler<CombatStartedEventArgs>? CombatStarted;
        public event EventHandler<CombatDeletedEventArgs>? CombatDeleted;
        public event EventHandler<CombatUpdatedEventArgs>? CombatUpdated;
        public event EventHandler<CombatantCreatedEventArgs>? CombatantCreated;
        public event EventHandler<CombatantUpdatedEventArgs>? CombatantUpdated;
        public event EventHandler<CombatantDeletedEventArgs>? CombatantDeleted;
    }
}
