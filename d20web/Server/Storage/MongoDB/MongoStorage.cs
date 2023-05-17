using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace d20Web.Storage.MongoDB
{
    /// <summary>
    /// Base class for mongo storage classes
    /// </summary>
    public abstract class MongoStorage
    {
        /// <summary>
        /// Constructor for <see cref="MongoStorage"/>
        /// </summary>
        /// <param name="settings">Settings for contacting MongoDB</param>
        protected MongoStorage(StorageSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
        private readonly StorageSettings _settings;
        private MongoClient? _client;
        private IMongoDatabase? _database;

        protected static readonly InsertOneOptions InsertOneOptions = new InsertOneOptions();
        protected static readonly ReplaceOptions ReplaceOptions = new ReplaceOptions();
        protected static readonly InsertManyOptions InsertManyOptions = new InsertManyOptions();

        [MemberNotNull(nameof(_client)), MemberNotNull(nameof(_database))]
        public Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (_client == null || _database == null)
            {
                _client = new MongoClient(_settings.ConnectionString);
                _database = _client.GetDatabase(_settings.DatabaseName);
            }
            return Task.CompletedTask;
        }

        protected async Task<IMongoDatabase> GetDatabase(CancellationToken cancellationToken = default)
        {
            if (_database == null)
                await ConnectAsync(cancellationToken);

            return _database;
        }
    }
}
