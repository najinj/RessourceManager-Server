

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RessourceManager.Core.Models.V1
{
    public class ReservationSettings
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int MaxNumberOfReservationsPerUser { get; set; }
        public int MaxDurationPerReservation { get; set; }
        public int MaxNumberOfReservationsAtSamePeriodPerUser { get; set; }
        public int IntervalAllowedForReservations { get; set; }
    }
}
