using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


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
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
    public class ReservationSettings
    {
        public int MaxNumberOfReservationsPerUser { get; set; }
        public int MaxDurationPerReservation { get; set; }
        public int MaxNumberOfReservationsAtSamePeriodPerUser { get; set; }
        public int IntervalAllowedForReservations { get; set; }
    }

    public class CalendarSettings
    {
        public string minTime { get; set; }
        public string maxTime { get; set; }
        public string firstDay { get; set; }
        public string HourSlotLabelFormat { get; set; }
        public string MinuteSlotLabelFormat { get; set; }
        public bool Hour12SlotLabelFormat { get; set; }
    }
}
