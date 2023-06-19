using d20Web.Models;
using d20Web.Models.Players;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public class MongoPlayerCharacter : INamedObject
    {
        #region Constructors
        public MongoPlayerCharacter()
        {
        }
        public MongoPlayerCharacter(PlayerCharacter character)
        {
            Name = character.Name;
            Player = character.Player;
            IncludeInCombat = character.IncludeInCombat;
            HitDie = character.HitDie;
            RollingStrategy = character.RollingStrategy;
            InitiativeModifier = character.InitiativeModifier;
            LightRadius = character.LightRadius;
            Senses = character.Senses;
            Languages = character.Languages;
            Alignment = character.Alignment;
            Notes = character.Notes;
        }
        #endregion
        #region Properties
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId CampaignID { get; set; }
        public string? Name { get; set; }
        public string? Player { get; set; }
        public bool IncludeInCombat { get; set; }
        public string? HitDie { get; set; }
        public RollingStrategy RollingStrategy { get; set; }
        public int InitiativeModifier { get; set; }
        public int LightRadius { get; set; }
        public string? Senses { get; set; }
        public string? Languages { get; set; }
        public Alignment Alignment { get; set; }
        public string[]? Notes { get; set; }
        #endregion
        #region Methods
        public PlayerCharacter ToCharacter()
        {
            return new PlayerCharacter()
            {
                ID = ID.ToString(),
                Name = Name,
                Player = Player,
                IncludeInCombat = IncludeInCombat,
                HitDie = HitDie,
                RollingStrategy = RollingStrategy,
                InitiativeModifier = InitiativeModifier,
                LightRadius = LightRadius,
                Senses = Senses,
                Languages = Languages,
                Alignment = Alignment,
                Notes = Notes,
            };
        }

        internal MongoCombatantPrep ToCombatantPrep()
        {
            return new MongoCombatantPrep()
            {
                PlayerID = ID,
                InitiativeModifier = InitiativeModifier,
                InitiativeRoll = 0,
                IsPlayer = true,
                Name = Name,
                Ordinal = 1,
            };
        }
        #endregion
    }
}
