using d20Web.Helpers;
using d20Web.Models;
using d20Web.Models.Combat;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public class MongoScenarioCombatant : INamedObject
    {
        public MongoScenarioCombatant() { }
        public MongoScenarioCombatant(CombatantTemplate template)
        {
            Name = template.Name;
            HitDieString = template.HitDieString;
            HitDieRollingStrategy = template.HitDieRollingStrategy;
            InitiativeModifier = template.InitiativeModifier;
            Count = template.Count;
            DisplayToPlayers = template.DisplayToPlayers;
            DisplayName = template.DisplayName;
            FastHealing = template.FastHealing;
            DeadAt = template.DeadAt;
            UnconsciousAt = template.UnconsciousAt;
            DamageReduction = template.DamageReduction;
            if (template.SourceID != null && ObjectId.TryParse(template.SourceID, out ObjectId source))
                SourceID = source;

        }
        /// <summary>
        /// Gets the ID of this combatant
        /// </summary>
        [BsonId]
        public ObjectId ID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the campaign
        /// </summary>
        public ObjectId CampaignID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the scenario
        /// </summary>
        public ObjectId ScenarioID { get; set; }
        /// <summary>
        /// Gets or sets the name of the combatant
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the string used for Hit Dice
        /// </summary>
        public string? HitDieString { get; set; }
        /// <summary>
        /// Gets or sets the rolling strategy to use
        /// </summary>
        public RollingStrategy HitDieRollingStrategy { get; set; }
        /// <summary>
        /// Gets or sets the default initiative modifier for this combatant
        /// </summary>
        public int InitiativeModifier { get; set; }
        /// <summary>
        /// Gets or sets the number of combatants to add
        /// </summary>
        public string? Count { get; set; }
        /// <summary>
        /// Gets or sets whether or not to display this combatant to the players
        /// </summary>
        public bool DisplayToPlayers { get; set; }
        /// <summary>
        /// Gets or sets the name to display to the players during combat
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the amount of fast healing the combatant will have
        /// </summary>
        public int FastHealing { get; set; }
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is dead at
        /// </summary>
        public int DeadAt { get; set; }
        /// <summary>
        /// Gets or sets the amount of hit points the combatant is unconscious at
        /// </summary>
        public int UnconsciousAt { get; set; }
        /// <summary>
        /// Gets or sets the source for this combatant template, or null if no source.
        /// </summary>
        public ObjectId SourceID { get; set; }
        /// <summary>
        /// Gets a collection of damage reductions
        /// </summary>
        public DamageReduction[]? DamageReduction { get; set; }

        public CombatantTemplate ToTemplate()
        {
            return new CombatantTemplate()
            {
                ID = ID.ToString(),
                DamageReduction = DamageReduction,
                DeadAt = DeadAt,
                DisplayName = DisplayName,
                DisplayToPlayers = DisplayToPlayers,
                HitDieRollingStrategy = HitDieRollingStrategy,
                HitDieString = HitDieString,
                SourceID = SourceID.ToString(),
                Count = Count,
                FastHealing = FastHealing,
                InitiativeModifier = InitiativeModifier,
                Name = Name,
                UnconsciousAt = UnconsciousAt,
            };
        }
    }

    internal static class MongoScenarioCombatantExtensions
    {
        public static MongoCombatantPrep ToCombatPrep(this MongoScenarioCombatant combatant)
        {
            return new MongoCombatantPrep()
            {
                BestiaryID = combatant.SourceID,
                CampaignID = combatant.CampaignID,
                InitiativeModifier = combatant.InitiativeModifier,
                IsPlayer = false,
                Name = combatant.Name,
            };
        }

        /// <summary>
        /// Creates combat prep objects for combatants
        /// </summary>
        /// <param name="combatants">Combatants to create combat prep objects for</param>
        /// <returns>Combat prep objects created.</returns>
        /// <remarks>
        /// If more than 1 is created of the same name, the ordinal is assigned starting at 1.  If there are existing combat prep objects
        /// with this name, this should be incremented as necessary to continue the number.
        /// </remarks>
        public static IEnumerable<MongoCombatantPrep> ToCombatPrep(this IEnumerable<MongoScenarioCombatant> combatants)
        {
            foreach (MongoScenarioCombatant source in combatants)
            {
                if (!string.IsNullOrWhiteSpace(source.Count))
                {
                    //  Need to roll for count, creat that many prep objects for this source
                    int count = Dice.Roll(source.Count, source.HitDieRollingStrategy);

                    for (int i = 0; i < count; i++)
                    {
                        var prep = source.ToCombatPrep();
                        if (count > 1)
                            prep.Ordinal = i + 1;

                        yield return prep;
                    }
                }
            }
        }
    }
}
