using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;




namespace RessourceManager.Core.Models.V1
{

    public enum Status { Chained, Unchained };
    public class Asset
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetTypeId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SpaceId { get; set; }  // An asset could be linked to a space
        public Status Status { get; set; } = Status.Unchained; // Default Value 
        [Required]
        public string Name { set; get; }
    }
    
}
