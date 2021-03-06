﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RessourceManager.Core.Models.V1;
using System;
using System.ComponentModel.DataAnnotations;


namespace RessourceManager.Core.ViewModels.RessourceType
{
    public class RequiredEnumAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value); ;
        }
    }
    public class RessourceTypeViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [RequiredEnum]
        public RType Type { get; set; } 
        public string Description { get; set; }
    }
}
