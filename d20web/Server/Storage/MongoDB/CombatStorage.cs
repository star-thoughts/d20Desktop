using d20Web.Models;
using d20Web.Storage.MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace d20Web.Storage.MongoDB
{
    /// <summary>
    /// Storage for handling active combats
    /// </summary>
    public sealed class CombatStorage : MongoStorage, ICombatStorage
    {
        /// <summary>
        /// Constructor for <see cref="CombatStorage"/>
        /// </summary>
        /// <param name="settings">Settings for contacting MongoDB</param>
        public CombatStorage(StorageSettings settings)
            : base(settings)
        {
        }

        private const string CombatsCollection = "combats";
        private const string CombatantsCollection = "combatants";

        private IMongoCollection<MongoCombat> GetCombatsCollection()
        {
            return GetDatabase().GetCollection<MongoCombat>(CombatsCollection);
        }

        private IMongoCollection<MongoCombatant> GetCombatantsCollection()
        {
            return GetDatabase().GetCollection<MongoCombatant>(CombatantsCollection);
        }

        /// <summary>
        /// Begins combat in the given campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="name">Name of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task<string> BeginCombat(string campaignID, string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId objectID))
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            IMongoCollection<MongoCombat> combatCollection = GetCombatsCollection();

            MongoCombat combat = new MongoCombat()
            {
                CampaignID = objectID,
                Name = name,
            };

            await combatCollection.InsertOneAsync(combat, InsertOneOptions, cancellationToken);

            return combat.ID.ToString();
        }
        /// <summary>
        /// Updates a given combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combat">Combat information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombat(string campaignID, Combat combat, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);
            if (string.IsNullOrWhiteSpace(combat?.ID))
                throw new ArgumentNullException(nameof(Combat.ID));
            if (!ObjectId.TryParse(combat.ID, out ObjectId combatObjectID))
                throw new ItemNotFoundException(ItemType.Combat, combat.ID);

            IMongoCollection<MongoCombat> combatCollection = GetCombatsCollection();

            MongoCombat mongoCombat = MongoCombat.Create(campaignObjectID, combatObjectID, combat);

            FilterDefinition<MongoCombat> filter = Builders<MongoCombat>.Filter
                .Eq(p => p.ID, combatObjectID);

            await combatCollection.FindOneAndReplaceAsync(filter, mongoCombat, null, cancellationToken);
        }

        /// <summary>
        /// Gets information for a combat
        /// </summary>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        public async Task<Combat> GetCombat(string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId objectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombat> combatCollection = GetCombatsCollection();

            FilterDefinition<MongoCombat> filter = Builders<MongoCombat>.Filter
                .Eq(p => p.ID, objectID);

            MongoCombat combat = await combatCollection.Find(filter)
                .Limit(1)
                .FirstOrDefaultAsync();

            if (combat == null)
                throw new ItemNotFoundException(ItemType.Combat, combatID);

            return combat.ToCombat();
        }


        /// <summary>
        /// Adds the given combatant to a combat
        /// </summary>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatants">Combatant information to add to the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the campaign, as well as the IDs of the newly created combatants</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null</exception>
        /// <exception cref="ArgumentException">One or more of the IDs could not be converted to a MongoDB ObjectID</exception>
        public async Task<(string, IEnumerable<string>)> CreateCombatants(string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(combatID));

            if (combatants == null)
                throw new ArgumentNullException(nameof(combatants));

            IMongoCollection<MongoCombatant> combatantsCollection = GetCombatantsCollection();
            IMongoCollection<MongoCombat> combatsCollection = GetCombatsCollection();

            FilterDefinition<MongoCombat> combatFilter = Builders<MongoCombat>.Filter
                .Eq(p => p.ID, combatObjectID);

            ObjectId campaignID = await combatsCollection.Find(combatFilter)
                .Project(p => p.CampaignID)
                .FirstOrDefaultAsync();

            if (ObjectId.Empty.Equals(campaignID))
                throw new ItemNotFoundException(ItemType.Combat, combatID);

            MongoCombatant[] dbCombatants = combatants.Select(p => MongoCombatant.Create(campaignID, combatObjectID, p)).ToArray();

            await combatantsCollection.InsertManyAsync(dbCombatants, InsertManyOptions, cancellationToken);

            return (campaignID.ToString(), dbCombatants.Select(p => p.ID.ToString()).ToArray());
        }

        /// <summary>
        /// Updates the details of a combatant
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat containing the combatant</param>
        /// <param name="combatant">Combatant information to use to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatant(string campaignID, string combatID, Combatant combatant, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ItemNotFoundException(ItemType.Campaign, campaignID)
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ItemNotFoundException(ItemType.Combat, combatID);

            if (combatant == null)
                throw new ArgumentNullException(nameof(combatant));

            if (string.IsNullOrEmpty(combatant.ID))
                throw new ArgumentNullException(nameof(combatant.ID));
            if (!ObjectId.TryParse(combatant.ID, out ObjectId combatantObjectID))
                throw new ItemNotFoundException(ItemType.Combatant, combatant.ID);

            IMongoCollection<MongoCombat> combatCollection = GetCombatsCollection();
            IMongoCollection<MongoCombatant> combatantsCollection = GetCombatantsCollection();

            FilterDefinition<MongoCombat> combatFilter = Builders<MongoCombat>.Filter
                .Eq(p => p.CampaignID, campaignObjectID) & Builders<MongoCombat>.Filter.Eq(p => p.ID, combatObjectID);

            if (!await combatCollection.Find(combatFilter).AnyAsync())
                throw new ItemNotFoundException(ItemType.Combat, combatID);

            FilterDefinition<MongoCombatant> combatantsFilter = Builders<MongoCombatant>.Filter
                .Eq(p => p.ID, combatantObjectID);

            await combatantsCollection.FindOneAndReplaceAsync(combatantsFilter, MongoCombatant.Create(campaignObjectID, combatObjectID, combatant), null, cancellationToken);
        }

        /// <summary>
        /// Gets combatant information
        /// </summary>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantID">ID of the combatant</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<Combatant> GetCombatant(string combatID, string combatantID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(combatID));
            if (string.IsNullOrWhiteSpace(combatantID))
                throw new ArgumentNullException(nameof(combatantID));
            if (!ObjectId.TryParse(combatantID, out ObjectId combatantObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(combatantID));

            IMongoCollection<MongoCombatant> combatantsCollection = GetCombatantsCollection();

            FilterDefinition<MongoCombatant> filter = Builders<MongoCombatant>.Filter
                .Eq(p => p.ID, combatantObjectID);

            Combatant combatant = await combatantsCollection.Find(filter)
                .Limit(1)
                .Project(p => p.ToCombatant())
                .FirstOrDefaultAsync();

            if (combatant == null)
                throw new ItemNotFoundException(ItemType.Combatant, combatantID);

            return combatant;
        }

        /// <summary>
        /// Gets a complete list of all combatants in a combat
        /// </summary>
        /// <param name="combatID">ID of the combat to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combatants in the given combat</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        /// <exception cref="ArgumentException">One or more IDs could not be converted to a MongoDB ObjectId</exception>
        public async Task<IEnumerable<Combatant>> GetCombatants(string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(combatID));

            IMongoCollection<MongoCombatant> combatantsCollection = GetCombatantsCollection();

            FilterDefinition<MongoCombatant> filter = Builders<MongoCombatant>.Filter
                .Eq(p => p.CombatID, combatObjectID);

            List<Combatant> combatants = await combatantsCollection.Find(filter)
                .Project(p => p.ToCombatant())
                .ToListAsync();

            return combatants;
        }
    }
}
