﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace RessourceManager.Core.Models.V1
{
    public class Space
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // [RegularExpression(@"^(?=[a-f\d]{24}$)(\d+[a-f]|[a-f]+\d)i", ErrorMessage = "Not A valid RessourceTypeId")]
        [Required(ErrorMessage = "Ressource Type is Required")]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "Not a valid RessourceTypeId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SpaceTypeId { get; set; }
        public string SpaceTypeName { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string[] Tags { get; set; } = new string[] { };
        public ICollection<Asset> assests { get; set; } = new List<Asset>();
    }
}
