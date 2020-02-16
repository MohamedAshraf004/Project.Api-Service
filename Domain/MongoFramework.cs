using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Project.Api.Domain
{
    public class MongoFramework
    {
        public MongoFramework()
        {
            Dones = new List<MongoDone>() { };
            ToDos = new List<MongoToDo>() { };
            InProgress = new List<MongoInProgress>() { };
        }
        //[BsonId]
        //[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public List<MongoDone> Dones { get; set; }

        public List<MongoToDo> ToDos { get; set; }

        public List<MongoInProgress> InProgress { get; set; }

    }
}