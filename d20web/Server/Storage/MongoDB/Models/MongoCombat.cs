using Amazon.Runtime.Internal.Util;
using d20Web.Models.Combat;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public sealed class MongoCombat
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId CampaignID { get; set; }
        public string? Name { get; set; }
        public int Round { get; set; }

        public static MongoCombat Create(ObjectId campaignID, ObjectId combatID, Combat combat)
        {
            return new MongoCombat()
            {
                ID = combatID,
                CampaignID = campaignID,
                Name = combat.Name,
                Round = combat.Round,
            };
        }

        public Combat ToCombat()
        {
            return new Combat()
            {
                ID = ID.ToString(),
                Name = Name,
                Round = Round,
            };
        }
    }
}
