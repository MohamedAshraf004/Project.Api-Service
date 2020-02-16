using MongoDB.Bson.Serialization.Attributes;

namespace Project.Api.Domain
{
    public interface MongoIUser
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        string Id { get; set; }
        string Name { get; set; }

    }
}
