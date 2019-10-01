using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;



namespace RessourceManager.Core.Models.V1
{
    public enum RType { Space, Asset };
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
