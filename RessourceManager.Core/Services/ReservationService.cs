using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;
using System;
using RessourceManager.Core.Exceptions.Reservation;
using RessourceManager.Core.Helpers;

namespace RessourceManager.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IErrorHandler _errorHandler;
        public ReservationService(IReservationRepository reservationRepository, IErrorHandler errorHandler)
        {
            _reservationRepository = reservationRepository;
            _errorHandler = errorHandler;
        }

        public async Task<Reservation> Add(Reservation reservationIn)
        {
            var availability = await _reservationRepository.CheckAvailability(reservationIn.Start, reservationIn.End, reservationIn.ResourceId);
            if(!availability)
                throw new ReservationServiceException(_errorHandler.GetMessage(ErrorMessagesEnum.NotAvailable)
                        , new string[] { nameof(Reservation.Start), nameof(Reservation.End) });
            await _reservationRepository.Add(reservationIn);
            return reservationIn;
        }

        public async Task<IEnumerable<Reservation>> Add(IEnumerable<Reservation> reservationsIn)
        {
            foreach (var reservation in reservationsIn)
            {
                var availability = await _reservationRepository.CheckAvailability(reservation.Start, reservation.End, reservation.ResourceId);
                if(!availability)
                    throw new ReservationServiceException(_errorHandler.GetMessage(ErrorMessagesEnum.NotAvailable)
                       , new string[] { nameof(Reservation.Start), nameof(Reservation.End) });
            }
            await _reservationRepository.Add(reservationsIn);
            return reservationsIn;
        }

        public async Task<List<Reservation>> Get(DateTime start)
        {
            var reservations = await _reservationRepository.Get(start);
            return reservations;
        }

        public async Task<List<Reservation>> Get()
        {
            var reservations = await _reservationRepository.GetAll();
            return reservations.ToList();
        }

        public async Task<Reservation> Get(string reservationId)
        {
            var reservation = await _reservationRepository.GetById(reservationId);
            return reservation;
        }

        public async Task Remove(string id)
        {
            await _reservationRepository.Remove(id);
        }

        public async Task RemovePeriodicReservations(string periodicId)
        {
            await _reservationRepository.RemovePeriodicReservations(periodicId);
        }

        public Task Update(Reservation reservationIn)
        {
            throw new System.NotImplementedException();
        }
    }
}
