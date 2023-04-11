using d20Web.Models;
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

        private async Task<IMongoCollection<MongoCampaign>> GetCampaignsCollection()
        {
            return (await GetDatabase()).GetCollection<MongoCampaign>(CampaignsCollection);
        }
        
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

            await collection.InsertOneAsync(campaign, InsertOneOptions, cancellationToken);

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
    }
}
