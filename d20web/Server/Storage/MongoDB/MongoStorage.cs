using d20Web.Storage.MongoDB.Models;
using MongoDB.Bson;
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
        protected static readonly ReplaceOptions UpsertOptions = new ReplaceOptions() { IsUpsert = true };
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
        #region Helpers
        protected async Task<string> CreateNamedObject<T>(string collectionName, ItemType itemType, string campaignID, T namedObject, CancellationToken cancellationToken) where T : INamedObject
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException("Invalid campaign ID", nameof(campaignID));

            IMongoCollection<T> collection = (await GetDatabase()).GetCollection<T>(collectionName);

            namedObject.CampaignID = campaignObjectID;

            try
            {
                await collection.InsertOneAsync(namedObject, InsertOneOptions, cancellationToken);
            }
            catch (MongoDuplicateKeyException)
            {
                throw new ItemNameInUseException(itemType, namedObject.Name ?? string.Empty);
            }

            return namedObject.ID.ToString();
        }

        protected async Task ReplaceNamedObject<T>(string collectionName, ItemType itemType, string campaignID, string objectID, T namedObject, CancellationToken cancellationToken) where T : INamedObject
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException("Invalid campaign ID", nameof(campaignID));

            if (string.IsNullOrWhiteSpace(objectID))
                throw new ArgumentNullException(nameof(objectID));
            if (!ObjectId.TryParse(objectID, out ObjectId namedObjectID))
                throw new ArgumentException("Invalid object ID", nameof(objectID));

            IMongoCollection<T> collection = (await GetDatabase()).GetCollection<T>(collectionName);

            namedObject.CampaignID = campaignObjectID;
            namedObject.ID = namedObjectID;

            FilterDefinition<T> filter = Builders<T>.Filter
                .Eq(p => p.ID, namedObjectID);

            try
            {
                var result = await collection.ReplaceOneAsync(filter, namedObject, UpsertOptions, cancellationToken);
                if (result.MatchedCount == 0)
                    throw new ItemNotFoundException(itemType, objectID);
            }
            catch (MongoDuplicateKeyException)
            {
                throw new ItemNameInUseException(itemType, namedObject.Name ?? string.Empty);
            }
        }

        protected async Task<T> GetNamedObject<T>(string collectionName, ItemType itemType, string campaignID, string objectID, CancellationToken cancellationToken) where T : INamedObject
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException("Invalid campaign ID", nameof(campaignID));

            if (string.IsNullOrWhiteSpace(objectID))
                throw new ArgumentNullException(nameof(objectID));
            if (!ObjectId.TryParse(objectID, out ObjectId namedObjectID))
                throw new ArgumentException("Invalid object ID", nameof(objectID));

            IMongoCollection<T> collection = (await GetDatabase()).GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter
                .Eq(p => p.ID, namedObjectID) & Builders<T>.Filter.Eq(p => p.CampaignID, campaignObjectID);

            T result = await collection.Find(filter)
                .Limit(1)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                throw new ItemNotFoundException(itemType, objectID);

            return result;
        }

        protected async Task DeleteNamedObject<T>(string collectionName, string campaignID, string objectID, CancellationToken cancellationToken) where T : INamedObject
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException("Invalid campaign ID", nameof(campaignID));

            if (string.IsNullOrWhiteSpace(objectID))
                return;
            if (!ObjectId.TryParse(objectID, out ObjectId namedObjectID))
                return;

            IMongoCollection<T> collection = (await GetDatabase()).GetCollection<T>(collectionName);

            FilterDefinition<T> filter = Builders<T>.Filter
                .Eq(p => p.ID, namedObjectID) & Builders<T>.Filter.Eq(p => p.CampaignID, campaignObjectID);

            await collection.DeleteOneAsync(filter);
        }

        protected async Task<IEnumerable<T>> GetNamedObjectList<T>(string collectioName, string campaignID, CancellationToken cancellationToken) where T : INamedObject
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException("Invalid campaign ID", nameof(campaignID));

            IMongoCollection<T> collection = (await GetDatabase()).GetCollection<T>(collectioName);

            FilterDefinition<T> filter = Builders<T>.Filter
                .Eq(p => p.CampaignID, campaignObjectID);

            return await collection.Find(filter)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
