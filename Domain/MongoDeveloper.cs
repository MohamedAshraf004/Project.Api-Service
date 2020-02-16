using MongoDB.Bson.Serialization.Attributes;

namespace Project.Api.Domain
{
    public class MongoDeveloper : MongoIUser
    {
        //[BsonId]
        //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

    }
}