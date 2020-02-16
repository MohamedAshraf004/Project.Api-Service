using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Api.Domain.MongoDomains
{
    public class MongoProject
    {
        public MongoProject()
        {
            Developer = new List<MongoDeveloper>();
        }
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLogo { get; set; }
        public string ProjectPath { get; set; }
        public string Wiki { get; set; }

        [Range(55, 100)]
        public double Mark { get; set; }
        public MongoFramework Framework { get; set; }
        public List<MongoDeveloper> Developer { get; set; }
        public MongoSuperVisor SuperVisior { get; set; }
        public MongoEvalution Evalution { get; set; }
    }
}
