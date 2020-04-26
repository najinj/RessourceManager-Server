using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;


namespace RessourceManager.Core.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> Get();
        Task<List<Reservation>> Get(DateTime start);
        Task<Reservation> Get(string reservationId);
        Task<List<Reservation>> GetPeriodicReservations(string periodicId);
        Task<Reservation> Add(Reservation reservationIn);
        Task<IEnumerable<Reservation>> Add(IEnumerable<Reservation> reservationsIn);
        Task Update(Reservation reservationIn);
        Task Remove(string reservationId,string userId,bool isAdmin);
        Task RemovePeriodicReservations(string periodicId, string userId, bool isAdmin);
        Task<dynamic> Availability(DateTime start, DateTime end, RType resourceType,string[] resourceSubTypes,IEnumerable<DateTime> occurrences);
        Task<List<Reservation>> GetReservationsByResource(string resourceId, DateTime? startDate);
        Task<List<Reservation>> GetUserReservations(string userId);

    }
}
