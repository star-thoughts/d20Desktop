using d20Web.Hubs;
using d20Web.Models.Combat;

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
        public CombatClient(string baseUri, ILoggerProvider loggingProvider)
            : this(new Uri(baseUri), loggingProvider)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="CombatClient"/>
        /// </summary>
        /// <param name="baseUri">Base URI for connecting to the hub</param>
        /// <param name="loggingProvider">Logging provider</param>
        public CombatClient(Uri baseUri, ILoggerProvider loggingProvider)
            : base(new Uri($"{baseUri}hub/campaign"), loggingProvider)
        {
            #region Combat Prep
            AddMessageHandler<CombatPrepStartedEventArgs, string, string>(Constants.CombatPrepCreated, p => CombatPrepStarted?.Invoke(this, p));
            AddMessageHandler<CombatDeletedEventArgs, string, string>(Constants.CombatPrepDeleted, p => CombatPrepDeleted?.Invoke(this, p));
            AddMessageHandler<CombatantCreatedEventArgs, string, string, IEnumerable<string>>(Constants.CombatantPrepCreated, p => CombatantPrepCreated?.Invoke(this, p));
            AddMessageHandler<CombatantPrepUpdatedEventArgs, string, string, CombatantPreparer>(Constants.CombatantPrepUpdated, p => CombatantPrepUpdated?.Invoke(this, p));
            AddMessageHandler<CombatantDeletedEventArgs, string, string, string>(Constants.CombatantPrepsDeleted, p => CombatantPrepDeleted?.Invoke(this, p));
            #endregion
            #region Combat
            AddMessageHandler<CombatStartedEventArgs, string, string, string>(Constants.CombatCreated, p => CombatStarted?.Invoke(this, p));
            AddMessageHandler<CombatDeletedEventArgs, string, string>(Constants.CombatDeleted, p => CombatDeleted?.Invoke(this, p));
            AddMessageHandler<CombatUpdatedEventArgs, string, Combat>(Constants.CombatUpdated, p => CombatUpdated?.Invoke(this, p));
            AddMessageHandler<CombatantCreatedEventArgs, string, string, IEnumerable<string>>(Constants.CombatantCreated, p => CombatantCreated?.Invoke(this, p));
            AddMessageHandler<CombatantUpdatedEventArgs, string, string, Combatant>(Constants.CombatantUpdated, p => CombatantUpdated?.Invoke(this, p));
            AddMessageHandler<CombatantDeletedEventArgs, string, string, string>(Constants.CombatantsDeleted, p => CombatantDeleted?.Invoke(this, p));
            #endregion
        }

        #region Combat Prep
        public event EventHandler<CombatPrepStartedEventArgs>? CombatPrepStarted;
        public event EventHandler<CombatDeletedEventArgs>? CombatPrepDeleted;
        public event EventHandler<CombatantCreatedEventArgs>? CombatantPrepCreated;
        public event EventHandler<CombatantPrepUpdatedEventArgs>? CombatantPrepUpdated;
        public event EventHandler<CombatantDeletedEventArgs>? CombatantPrepDeleted;
        #endregion
        #region Combat
        public event EventHandler<CombatStartedEventArgs>? CombatStarted;
        public event EventHandler<CombatDeletedEventArgs>? CombatDeleted;
        public event EventHandler<CombatUpdatedEventArgs>? CombatUpdated;
        public event EventHandler<CombatantCreatedEventArgs>? CombatantCreated;
        public event EventHandler<CombatantUpdatedEventArgs>? CombatantUpdated;
        public event EventHandler<CombatantDeletedEventArgs>? CombatantDeleted;
        #endregion
    }
}
