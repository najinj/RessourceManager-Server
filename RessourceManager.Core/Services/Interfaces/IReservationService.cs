using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;


namespace RessourceManager.Core.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> Get();
        Task<Reservation> Get(string reservationIn);
        Task<Reservation> Create(Reservation reservationIn);
        Task Update(Reservation reservationIn);
        Task Remove(string id);
        Task RemovePeriodicReservations(string periodicId);

    }
}
