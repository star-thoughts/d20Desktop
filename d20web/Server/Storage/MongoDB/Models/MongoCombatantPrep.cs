using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using d20Web.Models.Combat;

namespace d20Web.Storage.MongoDB.Models
{
    internal sealed class MongoCombatantPrep : INamedObject
    {
        public MongoCombatantPrep() { }
        public MongoCombatantPrep(CombatantPreparer combatant, ObjectId campaignID, ObjectId combatID)
        {
            CampaignID = campaignID;
            CombatID = combatID;
            Name = combatant.Name;
            Ordinal = combatant.Ordinal;
            InitiativeRoll = combatant.InitiativeRoll;
            InitiativeModifier = combatant.InitiativeModifier;
            IsPlayer = combatant.IsPlayer;

            if (ObjectId.TryParse(combatant.BestiaryID, out ObjectId bestiaryID))
                BestiaryID = bestiaryID;
        }
        /// <summary>
        /// Gets or sets the ID of this combatant
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
        /// Gets or sets the name of this combatant
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets the ordinal for this combatant
        /// </summary>
        public int Ordinal { get; set; }
        /// <summary>
        /// Gets or sets the actual initiative roll
        /// </summary>
        public int InitiativeRoll { get; set; }
        /// <summary>
        /// Gets or sets the initiative modifier
        /// </summary>
        public int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets whether or not this is a player character
        /// </summary>
        public bool IsPlayer { get; set; }
        /// <summary>
        /// Gets or sets the ID of a monster in the bestiary this could be associated with, if any
        /// </summary>
        public ObjectId BestiaryID { get; set; }
        /// <summary>
        /// Gets or sets the ID of a player in the player database this could be associated with, if any
        /// </summary>
        public ObjectId PlayerID { get; set; }


        public CombatantPreparer ToCombatantPreparer()
        {
            return new CombatantPreparer()
            {
                ID = ID.ToString(),
                InitiativeModifier = InitiativeModifier,
                InitiativeRoll = InitiativeRoll,
                IsPlayer = IsPlayer,
                Name = Name,
                Ordinal = Ordinal,
                BestiaryID = BestiaryID.ToString(),
            };
        }
    }
}
