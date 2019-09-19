using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_mongo_auth.Models.RessourceTypes;

namespace test_mongo_auth.Models.Ressource
{
    public class Area 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AreaTypeId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string[] Tags { get; set; }
        public ICollection<Asset> assests { get; set; }
    }

}
