using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace test_mongo_auth.Models.RessourceTypes
{
    public enum RType  {Space,Asset};
    public class RessourceType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public RType Type { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
    }
    
}
