﻿using d20Web.Models.Combat;
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
        private const string CombatPrepCollection = "combatprep";
        private const string CombatantPrepCollection = "combatantprep";
        private const string CombatScenarioCollection = "scenarios";
        private const string ScenarioCombatantsCollection = "scenariocombatants";

        private async Task<IMongoCollection<MongoCombat>> GetCombatsCollection()
        {
            return (await GetDatabase()).GetCollection<MongoCombat>(CombatsCollection);
        }

        private async Task<IMongoCollection<MongoCombatant>> GetCombatantsCollection()
        {
            return (await GetDatabase()).GetCollection<MongoCombatant>(CombatantsCollection);
        }

        private async Task<IMongoCollection<MongoCombatPrep>> GetCombatPrepCollection()
        {
            return (await GetDatabase()).GetCollection<MongoCombatPrep>(CombatPrepCollection);
        }

        private async Task<IMongoCollection<MongoCombatantPrep>> GetCombatantPrepCollection()
        {
            return (await GetDatabase()).GetCollection<MongoCombatantPrep>(CombatantPrepCollection);
        }
        private async Task<IMongoCollection<MongoScenarioCombatant>> GetScenarioCombatantsCollection()
        {
            return (await GetDatabase()).GetCollection<MongoScenarioCombatant>(ScenarioCombatantsCollection);
        }
        #region Combat Prep
        /// <summary>
        /// Creates a new combat prep in a campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create the combat in</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combat that was created</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null or empty</exception>
        public async Task<string> CreateCombatPrep(string campaignID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));

            IMongoCollection<MongoCombatPrep> collection = await GetCombatPrepCollection();

            MongoCombatPrep combat = new MongoCombatPrep()
            {
                CampaignID = campaignObjectID,
            };

            await collection.InsertOneAsync(combat, InsertOneOptions, cancellationToken);

            return combat.ID.ToString();
        }
        /// <summary>
        /// Ends a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombatPrep> collection = await GetCombatPrepCollection();
            IMongoCollection<MongoCombatantPrep> combatantsCollection = await GetCombatantPrepCollection();

            FilterDefinition<MongoCombatPrep> filter = Builders<MongoCombatPrep>.Filter
                .Eq(p => p.ID, combatObjectID) & Builders<MongoCombatPrep>.Filter.Eq(p => p.CampaignID, campaignObjectID);

            await collection.DeleteOneAsync(filter, cancellationToken);

            FilterDefinition<MongoCombatantPrep> combatantFilter = Builders<MongoCombatantPrep>.Filter
                .Eq(p => p.CombatID, combatObjectID) & Builders<MongoCombatantPrep>.Filter.Eq(p => p.CampaignID, campaignObjectID);

            await combatantsCollection.DeleteManyAsync(combatantFilter, cancellationToken);
        }
        /// <summary>
        /// Gets a list of combat preps int he campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        public async Task<IEnumerable<CombatListData>> GetCombatPreparers(string campaignID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));

            FilterDefinition<MongoCombatPrep> filter = Builders<MongoCombatPrep>.Filter
                .Eq(p => p.CampaignID, campaignObjectID);

            IMongoCollection<MongoCombatPrep> collection = await GetCombatPrepCollection();

            List<MongoCombatPrep> result = await collection.Find(filter)
                .ToListAsync();

            return result.Select(p => p.ToCombatListData()).ToArray();
        }
        /// <summary>
        /// Gets information for a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign to create combat in</param>
        /// <param name="combatID">ID of the combat to get information for</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat information</returns>
        public async Task<CombatPrep> GetCombatPrep(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombatPrep> collection = await GetCombatPrepCollection();

            FilterDefinition<MongoCombatPrep> filter = Builders<MongoCombatPrep>.Filter
                .Eq(p => p.ID, combatObjectID) & Builders<MongoCombatPrep>.Filter.Eq(p => p.CampaignID, campaignObjectID);

            MongoCombatPrepWithCombatants result = await collection.Aggregate()
                .Match(filter)
                .Lookup<MongoCombatantPrep, MongoCombatPrepWithCombatants>(CombatantPrepCollection, nameof(MongoCombatPrep.ID), nameof(MongoCombatantPrep.CombatID), nameof(MongoCombatPrepWithCombatants.Combatants))
                .Limit(1)
                .FirstOrDefaultAsync();

            if (result == null)
                throw new ItemNotFoundException(ItemType.CombatPrep, combatID);

            return result.ToCombatPrep();
        }
        /// <summary>
        /// Creates a combatant prep with the given statistics
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to create the combatant in</param>
        /// <param name="combatants">Combatant information to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant</returns>
        public async Task<IEnumerable<string>> CreateCombatantPreparers(string campaignID, string combatID, IEnumerable<CombatantPreparer> combatants, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombatantPrep> collection = await GetCombatantPrepCollection();

            MongoCombatantPrep[] preparers = combatants.Select(p => new MongoCombatantPrep(p, campaignObjectID, combatObjectID)).ToArray();

            await collection.InsertManyAsync(preparers, InsertManyOptions, cancellationToken);

            return preparers.Select(p => p.ID.ToString()).ToArray();
        }
        /// <summary>
        /// Updates the stats for a combatant prep
        /// </summary>
        /// <param name="campaignID">Campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatant">Combatant details to use for the update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatantPreparer(string campaignID, string combatID, CombatantPreparer combatant, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombatantPrep> collection = await GetCombatantPrepCollection();

            FilterDefinition<MongoCombatantPrep> filter = Builders<MongoCombatantPrep>.Filter
                .Eq(p => p.CampaignID, campaignObjectID) & Builders<MongoCombatantPrep>.Filter.Eq(p => p.CombatID, combatObjectID);

            UpdateDefinition<MongoCombatantPrep> update = Builders<MongoCombatantPrep>.Update
                .Set(p => p.Name, combatant.Name)
                .Set(p => p.InitiativeModifier, combatant.InitiativeModifier)
                .Set(p => p.InitiativeRoll, combatant.InitiativeRoll);

            await collection.UpdateOneAsync(filter, update, new UpdateOptions(), cancellationToken);
        }
        /// <summary>
        /// Deletes the combatant from the combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteCombatantPreparers(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombatantPrep> collection = await GetCombatantPrepCollection();

            ObjectId[] combatants = combatantIDs.Select(p =>
            {
                ObjectId.TryParse(p, out ObjectId id);
                return id;
            }).ToArray();

            FilterDefinition<MongoCombatantPrep> filter = Builders<MongoCombatantPrep>.Filter.Eq(p => p.CampaignID, campaignObjectID)
                & Builders<MongoCombatantPrep>.Filter.Eq(p => p.CombatID, combatObjectID)
                & Builders<MongoCombatantPrep>.Filter.In(p => p.ID, combatants);

            await collection.DeleteManyAsync(filter, cancellationToken);
        }
        /// <summary>
        /// Gets the combatant preparers for a combat prep
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combatants requested</returns>
        public async Task<IEnumerable<CombatantPreparer>> GetCombatantPreparers(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException(nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException(nameof(combatID));

            IMongoCollection<MongoCombatantPrep> collection = await GetCombatantPrepCollection();

            ObjectId[] combatants = combatantIDs.Select(p =>
            {
                ObjectId.TryParse(p, out ObjectId id);
                return id;
            }).ToArray();

            FilterDefinition<MongoCombatantPrep> filter = Builders<MongoCombatantPrep>.Filter.Eq(p => p.CampaignID, campaignObjectID)
                & Builders<MongoCombatantPrep>.Filter.Eq(p => p.CombatID, combatObjectID);

            if (combatantIDs.Any())
            {
                filter = filter & Builders<MongoCombatantPrep>.Filter.In(p => p.ID, combatants);
            }

            List<MongoCombatantPrep> result = await collection.Find(filter)
                .Limit(combatantIDs.Count())
                .ToListAsync();

            return result.Select(p => p.ToCombatantPreparer()).ToArray();
        }
        #endregion
        #region Combat
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

            IMongoCollection<MongoCombat> combatCollection = await GetCombatsCollection();

            MongoCombat combat = new MongoCombat()
            {
                CampaignID = objectID,
                Name = name,
            };

            await combatCollection.InsertOneAsync(combat, InsertOneOptions, cancellationToken);

            return combat.ID.ToString();
        }
        /// <summary>
        /// Ends a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat to end</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task EndCombat(string campaignID, string combatID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));

            //  In this case, we succeed if the delete doesn't find any records
            if (ObjectId.TryParse(combatID, out ObjectId combatObjectID))
            {

                IMongoCollection<MongoCombat> combatCollection = await GetCombatsCollection();
                IMongoCollection<MongoCombatant> combatantsCollection = await GetCombatantsCollection();

                FilterDefinition<MongoCombat> filter = Builders<MongoCombat>.Filter
                    .Eq(p => p.ID, combatObjectID);

                await combatCollection.DeleteOneAsync(filter, cancellationToken);

                FilterDefinition<MongoCombatant> combatantsFilter = Builders<MongoCombatant>.Filter
                    .Eq(p => p.CombatID, combatObjectID);

                await combatantsCollection.DeleteManyAsync(combatantsFilter, cancellationToken);
            }
        }
        /// <summary>
        /// Gets a list of combats int he campaign
        /// </summary>
        /// <param name="campaignID">ID of the campaign</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Collection of combats in the campaign</returns>
        public async Task<IEnumerable<CombatListData>> GetCombats(string campaignID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);

            IMongoCollection<MongoCombat> combatsCollection = await GetCombatsCollection();

            FilterDefinition<MongoCombat> filter = Builders<MongoCombat>.Filter
                .Eq(p => p.CampaignID, campaignObjectID);

            return await combatsCollection.Find(filter)
                .Project(p => new CombatListData()
                {
                    ID = p.ID.ToString(),
                    Name = p.Name,
                })
                .ToListAsync(cancellationToken);
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

            IMongoCollection<MongoCombat> combatCollection = await GetCombatsCollection();

            MongoCombat mongoCombat = MongoCombat.Create(campaignObjectID, combatObjectID, combat);

            FilterDefinition<MongoCombat> filter = Builders<MongoCombat>.Filter
                .Eq(p => p.ID, combatObjectID);

            ReplaceOneResult result = await combatCollection.ReplaceOneAsync(filter, mongoCombat, new ReplaceOptions(), cancellationToken);

            if (result.MatchedCount == 0)
                throw new ItemNotFoundException(ItemType.Combat, combat.ID);
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

            IMongoCollection<MongoCombat> combatCollection = await GetCombatsCollection();

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
        /// <param name="campaignID">ID of the campaign containing the combat</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatants">Combatant information to add to the combat</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the campaign, as well as the IDs of the newly created combatants</returns>
        /// <exception cref="ArgumentNullException">One or more parameters was null</exception>
        /// <exception cref="ArgumentException">One or more of the IDs could not be converted to a MongoDB ObjectID</exception>
        public async Task<IEnumerable<string>> CreateCombatants(string campaignID, string combatID, IEnumerable<Combatant> combatants, CancellationToken cancellationToken = default)
        {

            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(combatID));

            if (combatants == null)
                throw new ArgumentNullException(nameof(combatants));

            IMongoCollection<MongoCombatant> combatantsCollection = await GetCombatantsCollection();
            IMongoCollection<MongoCombat> combatsCollection = await GetCombatsCollection();

            FilterDefinition<MongoCombat> combatFilter = Builders<MongoCombat>.Filter
                .Eq(p => p.ID, combatObjectID);

            if ((await combatsCollection.CountDocumentsAsync(combatFilter, null, cancellationToken)) == 0)
                throw new ItemNotFoundException(ItemType.Combat, combatID);

            MongoCombatant[] dbCombatants = combatants.Select(p => MongoCombatant.Create(campaignObjectID, combatObjectID, p)).ToArray();

            await combatantsCollection.InsertManyAsync(dbCombatants, InsertManyOptions, cancellationToken);

            return dbCombatants.Select(p => p.ID.ToString()).ToArray();
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
                throw new ItemNotFoundException(ItemType.Campaign, campaignID);
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

            IMongoCollection<MongoCombat> combatCollection = await GetCombatsCollection();
            IMongoCollection<MongoCombatant> combatantsCollection = await GetCombatantsCollection();

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

            IMongoCollection<MongoCombatant> combatantsCollection = await GetCombatantsCollection();

            FilterDefinition<MongoCombatant> filter = Builders<MongoCombatant>.Filter
                .Eq(p => p.ID, combatantObjectID);

            MongoCombatant combatant = await combatantsCollection.Find(filter)
                .Limit(1)
                .FirstOrDefaultAsync();

            if (combatant == null)
                throw new ItemNotFoundException(ItemType.Combatant, combatantID);

            return combatant.ToCombatant();
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

            IMongoCollection<MongoCombatant> combatantsCollection = await GetCombatantsCollection();

            FilterDefinition<MongoCombatant> filter = Builders<MongoCombatant>.Filter
                .Eq(p => p.CombatID, combatObjectID);

            List<MongoCombatant> combatants = await combatantsCollection.Find(filter)
                .ToListAsync();

            return combatants.Select(p => p.ToCombatant()).ToArray();
        }

        /// <summary>
        /// Deletes the combatant from the combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combat is in</param>
        /// <param name="combatID">ID of the combat the combatant is in</param>
        /// <param name="combatantIDs">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteCombatant(string campaignID, string combatID, IEnumerable<string> combatantIDs, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(campaignID))
                throw new ArgumentNullException(nameof(campaignID));
            if (!ObjectId.TryParse(campaignID, out ObjectId campaignObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(campaignID));
            if (string.IsNullOrWhiteSpace(combatID))
                throw new ArgumentNullException(nameof(combatID));
            if (!ObjectId.TryParse(combatID, out ObjectId combatObjectID))
                throw new ArgumentException("Invalid combat ID", nameof(combatID));

            IEnumerable<ObjectId> combatantObjectIDs = combatantIDs.Select(p =>
            {
                {
                    if (string.IsNullOrWhiteSpace(p))
                        throw new ArgumentNullException(nameof(p));
                    if (!ObjectId.TryParse(p, out ObjectId combatantObjectID))
                        throw new ArgumentException("Invalid combat ID", nameof(p));
                    return combatantObjectID;
                }
            }).ToArray();

            IMongoCollection<MongoCombatant> combatantsCollection = await GetCombatantsCollection();

            FilterDefinition<MongoCombatant> filter = Builders<MongoCombatant>.Filter.In(p => p.ID, combatantObjectIDs)
                & Builders<MongoCombatant>.Filter.Eq(p => p.CampaignID, campaignObjectID)
                & Builders<MongoCombatant>.Filter.Eq(p => p.CombatID, combatObjectID);

            await combatantsCollection.DeleteManyAsync(filter, cancellationToken);
        }
        #endregion
        #region Scenarios
        /// <summary>
        /// Creates a combat scenario to allow for creating combats
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario to create</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the created scenario</returns>
        public async Task<string> CreateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default)
        {
            MongoCombatScenario dbScenario = new MongoCombatScenario(scenario);

            return await CreateNamedObject(CombatScenarioCollection, ItemType.Scenario, campaignID, dbScenario, cancellationToken);
        }
        /// <summary>
        /// Update an existing scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenario">Scenario information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task UpdateCombatScenario(string campaignID, CombatScenario scenario, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(scenario.ID))
                throw new ArgumentNullException(nameof(scenario.ID));

            MongoCombatScenario dbScenario = new MongoCombatScenario(scenario);

            await ReplaceNamedObject(CombatScenarioCollection, ItemType.Scenario, campaignID, scenario.ID, dbScenario, cancellationToken);
        }
        /// <summary>
        /// Gets a scenario for creating a combat
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combat scenario information</returns>
        public async Task<CombatScenario> GetCombatScenario(string campaignID, string scenarioID, CancellationToken cancellationToken = default)
        {
            MongoCombatScenario result = await GetNamedObject<MongoCombatScenario>(CombatScenarioCollection, ItemType.Scenario, campaignID, scenarioID, cancellationToken);

            return result.ToScenario();
        }
        /// <summary>
        /// Deletes the given combat scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to delete</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteCombatScenario(string campaignID, string scenarioID, CancellationToken cancellationToken = default)
        {
            await DeleteNamedObject<MongoCombatScenario>(CombatScenarioCollection, campaignID, scenarioID, cancellationToken);

            if (ObjectId.TryParse(scenarioID, out ObjectId scenarioObjectID)) {
                IMongoCollection<MongoScenarioCombatant> combatantsCollection = await GetScenarioCombatantsCollection();

                FilterDefinition<MongoScenarioCombatant> filter = Builders<MongoScenarioCombatant>.Filter
                    .Eq(p => p.ScenarioID, scenarioObjectID);

                await combatantsCollection.DeleteManyAsync(filter, cancellationToken);
            }
        }
        /// <summary>
        /// Adds a combatant to a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to add to</param>
        /// <param name="template">Combatant information to add</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>ID of the combatant added</returns>
        public Task<string> AddScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default)
        {
            if (scenarioID == null || !ObjectId.TryParse(scenarioID, out ObjectId scenarioObjectID))
                throw new ArgumentException(nameof(scenarioID));

            MongoScenarioCombatant dbTemplate = new MongoScenarioCombatant(template);
            dbTemplate.ScenarioID = scenarioObjectID;

            return CreateNamedObject(ScenarioCombatantsCollection, ItemType.ScenarioCombatant, campaignID, dbTemplate, cancellationToken);
        }
        /// <summary>
        /// Updates a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario to update a combatant in</param>
        /// <param name="template">Combatant information to update</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task<string?> UpdateScenarioCombatant(string campaignID, string scenarioID, CombatantTemplate template, CancellationToken cancellationToken = default)
        {
            if (scenarioID == null || !ObjectId.TryParse(scenarioID, out ObjectId scenarioObjectID))
                throw new ArgumentException(nameof(scenarioID));

            MongoScenarioCombatant dbTemplate = new MongoScenarioCombatant(template);
            dbTemplate.ScenarioID = scenarioObjectID;

            string? id = await ReplaceNamedObject(ScenarioCombatantsCollection, ItemType.ScenarioCombatant, campaignID, scenarioID, dbTemplate, cancellationToken);
            return id;
        }
        /// <summary>
        /// Gets information for a combatant in a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario containing the combatant</param>
        /// <param name="templateID">ID of the combatant to get</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Combatant information</returns>
        public async Task<CombatantTemplate> GetScenarioCombatant(string campaignID, string scenarioID, string templateID, CancellationToken cancellationToken = default)
        {
            MongoScenarioCombatant result = await GetNamedObject<MongoScenarioCombatant>(ScenarioCombatantsCollection, ItemType.ScenarioCombatant, campaignID, templateID, cancellationToken);
            return result.ToTemplate();
        }
        /// <summary>
        /// Deletes a combatant from a scenario
        /// </summary>
        /// <param name="campaignID">ID of the campaign containing the scenario</param>
        /// <param name="scenarioID">ID of the scenario containing the combatant</param>
        /// <param name="templateID">ID of the combatant to remove</param>
        /// <param name="cancellationToken">Token for cancelling the operation</param>
        /// <returns>Task for asynchronous completion</returns>
        public async Task DeleteScenarioCombatant(string campaignID, string scenarioID, string templateID, CancellationToken cancellationToken = default)
        {
            await DeleteNamedObject<MongoScenarioCombatant>(ScenarioCombatantsCollection, campaignID, templateID, cancellationToken);
        }
        #endregion
    }
}
