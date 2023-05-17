using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace d20Web.Storage.MongoDB.Models
{
    public interface INamedObject
    {
        [BsonId]
        ObjectId ID { get; set; }
        ObjectId CampaignID { get; set; }
        string? Name { get; set; }
    }
}