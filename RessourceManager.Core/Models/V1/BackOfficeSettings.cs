using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;

namespace RessourceManager.Core.Models.V1
{
    public class BackOfficeSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public EmailSettings EmailSettings { get; set; }
        public ReservationSettings ReservationSettings { get; set; }
        public CalendarSettings CalendarSettings { get; set; }

    }

    public class EmailSettings
    {
        [DisplayName("MailServer")]
        [FieldTypeAttribute("String")]
        public string MailServer { get; set; }
        [DisplayName("MailPort")]
        [FieldTypeAttribute("Integer")]
        public int MailPort { get; set; }
        [DisplayName("SenderName")]
        [FieldTypeAttribute("String")]
        public string SenderName { get; set; }
        [DisplayName("Sender")]
        [FieldTypeAttribute("String")]
        public string Sender { get; set; }
        [DisplayName("Password")]
        [FieldTypeAttribute("String")]
        public string Password { get; set; }
    }
    public class ReservationSettings
    {
        [FieldTypeAttribute("Integer")]
        public int MaxNumberOfReservationsPerUser { get; set; }
        [FieldTypeAttribute("Integer")]
        public int MaxDurationPerReservation { get; set; }
        [FieldTypeAttribute("Integer")]
        public int MaxNumberOfReservationsAtSamePeriodPerUser { get; set; }
        [FieldTypeAttribute("Integer")]
        public int IntervalAllowedForReservations { get; set; }
    }

    public class CalendarSettings
    {
        [FieldTypeAttribute("Select")]
        public string minTime { get; set; }
        [FieldTypeAttribute("Select")]
        public string maxTime { get; set; }
        [FieldTypeAttribute("Select")]
        public int firstDay { get; set; }
        [FieldTypeAttribute("Select")]
        public string HourSlotLabelFormat { get; set; }
        [FieldTypeAttribute("Select")]
        public string MinuteSlotLabelFormat { get; set; }
        [FieldTypeAttribute("Boolean")]
        public bool Hour12SlotLabelFormat { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class FieldTypeAttributeAttribute : Attribute
    {
        private string name;
        public FieldTypeAttributeAttribute(string name)
        {
            this.name = name;
        }
        public string Name
        {
            get { return name; }
        }
    }

}
