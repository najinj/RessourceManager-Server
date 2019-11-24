using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Core.Repositories.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<List<Reservation>> Get(DateTime start);
        Task<List<Reservation>> GetReservationsByResource(string resourceId);
        Task<List<Reservation>> GetReservationsByInterval(DateTime start, DateTime end);
        Task Add(IEnumerable<Reservation> reservationsIn);
        Task RemovePeriodicReservations(string periodicId);
        Task<bool> CheckAvailability(DateTime start, DateTime end, string spaceId);
    }
}
