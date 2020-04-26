using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RessourceManager.Core.Models.V1
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [StringLength(24, MinimumLength = 24, ErrorMessage = "Not a valid ResourceId")]
        [Required]
        public string UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "Not a valid ResourceId")]
        [Required]
        public string ResourceId { get; set; }

        public string PeriodicId { get; set; }
        [Required]
        public RType ResourceType { get; set; }
        public string ResourceName { get; set; }

        public string ResourceTypeName { get; set; }

        public string Title { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
        public ICollection<int> DaysOfWeek { get; set; } = new List<int>();  // Built In parameter in the FullCallendar API
        /*
         A simalar field to allow to store the values for the days 
         of the week until functionality for DaysOfWeek is implemented 
         */
        public IEnumerable<int> WeekDays { get; set; } = new List<int>();
        public DateTime StartTime { get; set; } // When DaysOfWeek is set , StartTime is used to get the start time(HH:mm:ss) of the recuring event 
        public DateTime EndTime { get; set; }
        public DateTime StartRecur { get; set; }// When DaysOfWeek is set , StartRecur is used to get the start Date of the recuring event 
        public DateTime EndRecur { get; set; }


    }
}
