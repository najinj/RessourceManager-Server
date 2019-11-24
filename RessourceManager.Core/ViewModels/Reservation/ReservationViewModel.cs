

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RessourceManager.Core.ViewModels.Reservation
{
    public class ReservationViewModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "Not a valid ResourceId")]
        [Required]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "Not a valid ResourceId")]
        [Required]
        public string ResourceId { get; set; }
        
        [Required]
        public RType ResourceType { get; set; }
        public string Title { get; set; }
        
        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public string CronoExpression { get; set; }



    }
}
