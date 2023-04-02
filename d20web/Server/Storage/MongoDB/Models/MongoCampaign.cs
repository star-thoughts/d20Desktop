using d20Web.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public class MongoCampaign
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string? Name { get; set; }

        public Campaign ToCampaign()
        {
            return new Campaign()
            {
                ID = ID.ToString(),
                Name = Name,
            };
        }
    }
}
