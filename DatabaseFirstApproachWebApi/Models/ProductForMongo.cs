using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DatabaseFirstApproachWebApi.Models
{
    public class ProductForMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        //[BsonRepresentation(BsonType.Decimal128)]
        public string Company { get; set; }
    }
}
