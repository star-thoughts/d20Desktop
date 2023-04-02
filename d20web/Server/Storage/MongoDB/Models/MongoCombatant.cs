using d20Web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public sealed class MongoCombatant
    {
        /// <summary>
        /// Gets or sets the ID of the combatant
        /// </summary>
        [BsonId]
        public ObjectId ID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the combat this combatant is in
        /// </summary>
        public ObjectId CombatID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the campaign this combatant is in
        /// </summary>
        public ObjectId CampaignID { get; set; }
        /// <summary>
        /// Gets or sets the name of the combatant
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the name to display to players
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the ordinal for this combatant, for multiple combatants with the same name
        /// </summary>
        public int Ordinal { get; set; }
        /// <summary>
        /// Gets or sets the health related information for this combatant
        /// </summary>
        public CombatantHealth? Health { get; set; }
        /// <summary>
        /// Gets or sets the initiative order of this combatant
        /// </summary>
        public int InitiativeOrder { get; set; }
        /// <summary>
        /// Gets or sets whether or not to display this combatant to players
        /// </summary>
        public bool DisplayToPlayers { get; set; }
        /// <summary>
        /// Gets or sets whether or not this combatant is the current combatant
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// Gets or sets whether or not this combatant has gone at least once
        /// </summary>
        public bool HasGoneOnce { get; set; }
        /// <summary>
        /// Gets or sets whether or not this combatant is a player character
        /// </summary>
        public bool IsPlayer { get; set; }
        /// <summary>
        /// Gets or sets whether or not to include this combatant in combat (mainly to remove killed combatants)
        /// </summary>
        public bool IncludeInCombat { get; set; }
        /// <summary>
        /// Gets or sets a collection of descriptors for damage reduction for this combatant
        /// </summary>
        public IEnumerable<DamageReduction>? DamageReduction { get; set; }
        /// <summary>
        /// Gets or sets conditions that have been applied to this combatant
        /// </summary>
        public IEnumerable<AppliedCondition>? AppliedConditions { get; set; }

        /// <summary>
        /// Creates a MongoDB record from a combatant
        /// </summary>
        /// <param name="campaignID">ID of the campaign the combatant is in</param>
        /// <param name="combatID">ID of the combat</param>
        /// <param name="combatant">Information about the combatant</param>
        /// <returns></returns>
        public static MongoCombatant Create(ObjectId campaignID, ObjectId combatID, Combatant combatant)
        {
            MongoCombatant result = new MongoCombatant()
            {
                CampaignID = campaignID,
                CombatID = combatID,
                Name = combatant.Name,
                DisplayName = combatant.DisplayName,
                DamageReduction = combatant.DamageReduction,
                DisplayToPlayers = combatant.DisplayToPlayers,
                AppliedConditions = combatant.AppliedConditions,
                HasGoneOnce = combatant.HasGoneOnce,
                Health = combatant.Health,
                IncludeInCombat = combatant.IncludeInCombat,
                InitiativeOrder = combatant.InitiativeOrder,
                IsCurrent = combatant.IsCurrent,
                IsPlayer = combatant.IsPlayer,
                Ordinal = combatant.Ordinal,
            };

            return result;
        }

        /// <summary>
        /// Creates a combatant from a MongoDB record
        /// </summary>
        /// <returns>Combatant information</returns>
        public Combatant ToCombatant()
        {
            return new Combatant()
            {
                ID = ID.ToString(),
                DamageReduction = DamageReduction,
                DisplayToPlayers = DisplayToPlayers,
                Name = Name,
                DisplayName = DisplayName,
                AppliedConditions = AppliedConditions,
                HasGoneOnce = HasGoneOnce,
                Health = Health,
                IncludeInCombat = IncludeInCombat,
                InitiativeOrder = InitiativeOrder,
                IsCurrent = IsCurrent,
                IsPlayer = IsPlayer,
                Ordinal = Ordinal,
            };
        }
    }
}
