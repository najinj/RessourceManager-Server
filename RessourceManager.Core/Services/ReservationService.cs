using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;

namespace RessourceManager.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> Add(Reservation reservationIn)
        {
            var availability = await _reservationRepository.CheckAvailability(reservationIn.Start, reservationIn.End, null, reservationIn.ResourceId);
            if(availability)
               await _reservationRepository.Add(reservationIn);
            return reservationIn;
        }

        public async Task<IEnumerable<Reservation>> Add(IEnumerable<Reservation> reservationsIn)
        {
            var availability = false;
            foreach (var reservation in reservationsIn)
            {
                availability = await _reservationRepository.CheckAvailability(reservation.Start, reservation.End, null, reservation.ResourceId);
            }
            if (availability)
                await _reservationRepository.Add(reservationsIn);
            return reservationsIn;
        }

        public Task<List<Reservation>> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<Reservation> Get(string reservationIn)
        {
            throw new System.NotImplementedException();
        }

        public Task Remove(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task RemovePeriodicReservations(string periodicId)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(Reservation reservationIn)
        {
            throw new System.NotImplementedException();
        }
    }
}
