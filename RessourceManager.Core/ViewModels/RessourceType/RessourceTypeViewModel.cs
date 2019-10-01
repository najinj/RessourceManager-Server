using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RessourceManager.Core.Models.V1;
using System.ComponentModel.DataAnnotations;


namespace RessourceManager.Core.ViewModels.RessourceType
{
    public class RessourceTypeViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public RType Type { get; set; }
        public string Description { get; set; }
    }
}
