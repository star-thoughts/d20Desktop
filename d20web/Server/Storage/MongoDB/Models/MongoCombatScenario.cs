using d20Web.Models.Combat;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public class MongoCombatScenario : INamedObject
    {
        public MongoCombatScenario()
        {
        }

        public MongoCombatScenario(CombatScenario scenario)
        {
            Name = scenario.Name;
            Group = scenario.Group;
            Details = scenario.Details;
        }
        /// <summary>
        /// Gets the ID of this scenario
        /// </summary>
        [BsonId]
        public ObjectId ID { get; set; }
        /// <summary>
        /// Gets or sets the ID of the campaign
        /// </summary>
        public ObjectId CampaignID { get; set; }
        /// <summary>
        /// Gets or sets the name of this combat scenario
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the grouping for this scenario
        /// </summary>
        public string? Group { get; set; }
        /// <summary>
        /// Gets or sets any extra details for this combat scenario
        /// </summary>
        public string? Details { get; set; }

        public CombatScenario ToScenario()
        {
            return new CombatScenario()
            {
                ID = ID.ToString(),
                Name = Name,
                Details = Details,
                Group = Group,
            };
        }
    }
}
