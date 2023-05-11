using d20Web.Models.Bestiary;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace d20Web.Storage.MongoDB.Models
{
    /// <summary>
    /// MongoDB representation of a Monster in a bestiary
    /// </summary>
    public sealed class MongoMonster : INamedObject
    {
        public MongoMonster()
        {
        }

        public MongoMonster(Monster monster)
        {
            this.InitiativeModifier = monster.InitiativeModifier;
            this.UnconsciousAt = monster.UnconsciousAt;
            this.HitDieString = monster.HitDieString;
            this.DeadAt = monster.DeadAt;
            this.FastHealing = monster.FastHealing;
            this.Name = monster.Name;
            this.Stats = monster.Stats;
            this.Source = monster.Source;
        }

        /// <summary>
        /// Gets or sets the ID of the monster in the campaign
        /// </summary>
        public ObjectId ID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the campaign this monster is in
        /// </summary>
        public ObjectId CampaignID { get; set; }
        /// <summary>
        /// Gets or sets the name of this monster
        /// </summary>
        [Required]
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the stats for the monster
        /// </summary>
        [Required]
        public MonsterStat[]? Stats { get; set; }
        /// <summary>
        /// Gets or sets the hit dice of the monster
        /// </summary>
        [Required]
        public string? HitDieString { get; set; }
        /// <summary>
        /// Gets or sets the initiative modifier for the monster
        /// </summary>
        [Required]
        public int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets or sets the fast healing for the monster
        /// </summary>
        [Required]
        public int FastHealing { get; set; }
        /// <summary>
        /// Gets or sets the current HP the monster is considered dead at
        /// </summary>
        [Required]
        public int DeadAt { get; set; }
        /// <summary>
        /// Gets or sets the current HP the monster is considered unconscious at
        /// </summary>
        [Required]
        public int UnconsciousAt { get; set; }
        /// <summary>
        /// Gets or sets the source of this monster
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Creates a <see cref="Monster"/> object for communications
        /// </summary>
        /// <returns><see cref="Monster"/> object for communications</returns>
        public Monster ToMonster()
        {
            return new Monster()
            {
                ID = ID.ToString(),
                DeadAt = DeadAt,
                HitDieString = HitDieString,
                FastHealing = FastHealing,
                InitiativeModifier = InitiativeModifier,
                Name = Name,
                Source = Source,
                Stats = Stats,
                UnconsciousAt = UnconsciousAt,
            };
        }
    }
}