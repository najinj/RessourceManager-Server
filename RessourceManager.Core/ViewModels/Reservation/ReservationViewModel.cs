

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RessourceManager.Core.ViewModels.Reservation
{
    public class AvailabilityViewModel
    {
        [Required]
        public RType ResourceType { get; set; }

        public string[] ResourceSubTypes { get; set; } = new string[] { };

        [Required]
        [DateIntervalValidation]
        [DateNotInThePastValidation(ErrorMessage = "Can't book a reservation in the past")]
        public DateTime Start { get; set; }

        [Required]
        [DateIntervalValidation]
        [DateNotInThePastValidation(ErrorMessage = "Can't book a reservation in the past")]
        public DateTime End { get; set; }

        public string CronoExpression { get; set; }

    }

    public class ReservationViewModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24, ErrorMessage = "Not a valid ResourceId")]
        [Required]
        public string ResourceId { get; set; }

        [Required]
        public RType ResourceType { get; set; }

        public string[] ResourceSubTypes { get; set; } = new string[] {};
        public string Title { get; set; }

        [Required]
        [DateIntervalValidation]
        [DateNotInThePastValidation(ErrorMessage = "Can't book a reservation in the past")]
        public DateTime Start { get; set; }

        [Required]
        [DateIntervalValidation]
        [DateNotInThePastValidation(ErrorMessage ="Can't book a reservation in the past")]
        public DateTime End { get; set; }

        public string CronoExpression { get; set; }

        public string ResourceTypeName {
            get {
                return ResourceType.ToString();
            }
        }
        public IEnumerable<int> WeekDays { get;set;}

        public string ResourceName { get; set; }

    }


    [AttributeUsage(AttributeTargets.Property)]
    public class DateIntervalValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (IBackOfficeSettingsService)validationContext
                                 .GetService(typeof(IBackOfficeSettingsService));

            var settings =  service.Get().Result;

            var reservationSettings = settings.ReservationSettings;

            var inputValue = (DateTime)value;

            var limitDate = DateTime.UtcNow.AddDays(reservationSettings.IntervalAllowedForReservations);
            var diff = inputValue.Date.CompareTo(limitDate.Date);

            if (diff > 0)
            {
                return new ValidationResult($"Can't Add a reservation starting {reservationSettings.IntervalAllowedForReservations} days from today");
            }
            return null;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DateNotInThePastValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = (DateTime)value;
            if (DateTime.Today.Date.CompareTo(inputValue.Date) > 0)
                return false;
            return true;
        }
    }
}
