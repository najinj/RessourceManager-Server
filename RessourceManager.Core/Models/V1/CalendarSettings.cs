using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RessourceManager.Core.Models.V1
{
    public class CalendarSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string minTime { get; set; }
        public string maxTime { get; set; }
        public string firstDay { get; set; }
        public string HourSlotLabelFormat { get; set; }
        public string MinuteSlotLabelFormat { get; set; }
        public bool Hour12SlotLabelFormat { get; set; }
    }
}
