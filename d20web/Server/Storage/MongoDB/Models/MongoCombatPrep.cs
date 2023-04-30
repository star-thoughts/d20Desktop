using d20Web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    internal class MongoCombatPrep
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public ObjectId CampaignID { get; set; }

        public virtual CombatPrep ToCombatPrep()
        {
            return new CombatPrep()
            {
                ID = ID.ToString(),
            };
        }

        public CombatListData ToCombatListData()
        {
            return new CombatListData()
            {
                ID = ID.ToString(),
            };
        }
    }
}