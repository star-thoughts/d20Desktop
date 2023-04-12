using Microsoft.AspNetCore.SignalR.Client;

namespace d20Web.SignalRClient
{
    /// <summary>
    /// Base class for SignalR clients
    /// </summary>
    public abstract class ClientBase : IAsyncDisposable
    {
        public ClientBase(Uri uri, ILoggerProvider loggingProvider)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            _connection = new HubConnectionBuilder()
                .WithUrl(uri)
                .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    logging.AddProvider(loggingProvider);
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();

            _connection.Reconnecting += _connection_Reconnecting;
            _connection.Reconnected += _connection_Reconnected;
            _connection.Closed += _connection_Closed;
        }

        private readonly HubConnection _connection;

        /// <summary>
        /// Gets whether or not this is connected to a server
        /// </summary>
        public bool IsConnected { get { return _connection.State == HubConnectionState.Connected; } }

        protected void AddMessageHandler<TEventArgs, TArg1>(string messageName, Action<TEventArgs> action) where TEventArgs : EventArgs
        {
            _connection.On<TArg1>(messageName, p1 =>
            {
                TEventArgs? args = (TEventArgs?)Activator.CreateInstance(typeof(TEventArgs), p1);
                if (args != null)
                    action(args);
            });
        }

        protected void AddMessageHandler<TEventArgs, TArg1, TArg2>(string messageName, Action<TEventArgs> action) where TEventArgs : EventArgs
        {
            _connection.On<TArg1, TArg2>(messageName, (p1, p2) =>
            {
                TEventArgs? args = (TEventArgs?)Activator.CreateInstance(typeof(TEventArgs), p1, p2);
                if (args != null)
                    action(args);
            });
        }

        protected void AddMessageHandler<TEventArgs, TArg1, TArg2, TArg3>(string messageName, Action<TEventArgs> action) where TEventArgs : EventArgs
        {
            _connection.On<TArg1, TArg2, TArg3>(messageName, (p1, p2, p3) =>
            {
                TEventArgs? args = (TEventArgs?)Activator.CreateInstance(typeof(TEventArgs), p1, p2, p3);
                if (args != null)
                    action(args);
            });
        }

        public Task StartClient(CancellationToken cancellationToken = default)
        {
            return _connection.StartAsync(cancellationToken);
        }

        public Task StopClient(CancellationToken cancellationToken = default)
        {
            if (_connection.State == HubConnectionState.Connected)
                return _connection.StopAsync(cancellationToken);
            return Task.CompletedTask;
        }


        private Task _connection_Reconnecting(Exception? arg)
        {
            Reconnecting?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        private Task _connection_Reconnected(string? arg)
        {
            Reconnected?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        private Task _connection_Closed(Exception? arg)
        {
            Disconnected?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }
        /// <summary>
        /// Event that is triggerd when the server connection is lost and we are attepmting to reconnect
        /// </summary>
        public event EventHandler? Reconnecting;
        /// <summary>
        /// Event that is triggered when we have lost a server connection, but reconnected
        /// </summary>
        public event EventHandler? Reconnected;
        /// <summary>
        /// Event that is triggered when we have closed the connection
        /// </summary>
        public event EventHandler? Disconnected;

        /// <summary>
        /// Disposes of this object
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return _connection.DisposeAsync();
        }
    }
}
