using d20Web.Models;
using d20Web.Models.Bestiary;
using d20Web.Storage.MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace d20Web.Storage.MongoDB
{
    /// <summary>
    /// Stores campaign information in a MongoDB
    /// </summary>
    public sealed class CampaignStorage : MongoStorage, ICampaignStorage
    {
        public CampaignStorage(StorageSettings settings)
            : base(settings)
        {
        }

        private const string CampaignsCollection = "campaigns";
        private const string BestiaryCollection = "bestiary";

        private async Task<IMongoCollection<MongoCampaign>> GetCampaignsCollection()
        {
            return (await GetDatabase()).GetCollection<MongoCampaign>(CampaignsCollection);
        }
        #region Campaigns
        /// <summary>
        /// Creates a campaign
        /// </summary>
        /// <param name="name">Name of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the campaign that was created</returns>
        public async Task<string> CreateCampaign(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            IMongoCollection<MongoCampaign> collection = await GetCampaignsCollection();

            MongoCampaign campaign = new MongoCampaign()
            {
                Name = name,
            };

            try
            {
                await collection.InsertOneAsync(campaign, InsertOneOptions, cancellationToken);
            }
            catch (MongoDuplicateKeyException)
            {
                throw new ItemNameInUseException(ItemType.Campaign, name);
            }

            return campaign.ID.ToString();
        }
        /// <summary>
        /// Gets a collection of campaigns in the system
        /// </summary>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of campaign information</returns>
        public async Task<IEnumerable<CampaignListData>> GetCampaigns(CancellationToken cancellationToken = default)
        {
            IMongoCollection<MongoCampaign> collection = await GetCampaignsCollection();

            return await collection.Find(Builders<MongoCampaign>.Filter.Empty)
                .Project(p => new CampaignListData()
                {
                    Name = p.Name,
                    ID = p.ID.ToString(),
                })
                .ToListAsync(cancellationToken);
        }
        /// <summary>
        /// Gets the information for a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Campaign information retrieved</returns>
        public async Task<Campaign> GetCampaign(string campaignID, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId objectID))
                throw new ArgumentException("Invalid campaign ID", nameof(campaignID));

            IMongoCollection<MongoCampaign> collection = await GetCampaignsCollection();

            FilterDefinition<MongoCampaign> filter = Builders<MongoCampaign>.Filter
                .Eq(p => p.ID, objectID);

            Campaign? campaign = await collection.Find(filter)
                .Limit(1)
                .Project(p => p.ToCampaign())
                .FirstOrDefaultAsync();

            if (campaign == null)
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);

            return campaign;
        }
        #endregion
        #region Monsters
        private async Task<IMongoCollection<MongoMonster>> GetBestiaryCollection()
        {
            return (await GetDatabase()).GetCollection<MongoMonster>(BestiaryCollection);
        }
        /// <summary>
        /// Creates a monster in the bestiary
        /// </summary>
        /// <param name="campaignID">ID of the campaign to put the monster in</param>
        /// <param name="monster">Monster to add to the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the monster that was added</returns>
        public Task<string> CreateMonster(string campaignID, Monster monster, CancellationToken cancellationToken = default)
        {
            MongoMonster dbMonster = new MongoMonster(monster);

            return CreateNamedObject(BestiaryCollection, ItemType.Monster, campaignID, dbMonster, cancellationToken);
        }

        /// <summary>
        /// Gets the monster with the given ID
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="monsterID">ID of the monster to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Monster retrieved from DB</returns>
        public async Task<Monster> GetMonster(string campaignID, string monsterID, CancellationToken cancellationToken = default)
        {
            return (await GetNamedObject<MongoMonster>(BestiaryCollection, ItemType.Monster, campaignID, monsterID, cancellationToken)).ToMonster();
        }

        /// <summary>
        /// Updates the monster
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the monster</param>
        /// <param name="monster">Monster information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateMonster(string campaignID, Monster monster, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(monster.ID))
                throw new ItemNotFoundException(ItemType.Monster, string.Empty);

            await ReplaceNamedObject(BestiaryCollection, ItemType.Monster, campaignID, monster.ID, new MongoMonster(monster), cancellationToken);
        }

        /// <summary>
        /// Deletes the given monster from the database
        /// </summary>
        /// <param name="campaignID">ID of the campaign the monster is in</param>
        /// <param name="monsterID">ID of the monster to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteMonster(string campaignID, string monsterID, CancellationToken cancellationToken = default)
        {
            await DeleteNamedObject<MongoMonster>(BestiaryCollection, campaignID, monsterID, cancellationToken);
        }
        #endregion
        #region Helpers
        private async Task<string> CreateNamedObject<T>(string collectionName, ItemType itemType, string campaignID, T namedObject, CancellationToken cancellationToken) where T : INamedObject
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

        private async Task ReplaceNamedObject<T>(string collectionName, ItemType itemType, string campaignID, string objectID, T namedObject, CancellationToken cancellationToken) where T : INamedObject
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

            FilterDefinition<T> filter = Builders<T>.Filter
                .Eq(p => p.ID, namedObjectID);

            try
            {
                var result = await collection.ReplaceOneAsync(filter, namedObject, ReplaceOptions, cancellationToken);
                if (result.MatchedCount == 0)
                    throw new ItemNotFoundException(itemType, objectID);
            }
            catch (MongoDuplicateKeyException)
            {
                throw new ItemNameInUseException(itemType, namedObject.Name ?? string.Empty);
            }
        }

        private async Task<T> GetNamedObject<T>(string collectionName, ItemType itemType, string campaignID, string objectID, CancellationToken cancellationToken) where T : INamedObject
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

        private async Task DeleteNamedObject<T>(string collectionName, string campaignID, string objectID, CancellationToken cancellationToken) where T : INamedObject
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
        #endregion
    }
}
