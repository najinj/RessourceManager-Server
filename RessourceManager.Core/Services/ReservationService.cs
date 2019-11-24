﻿using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;
using System;

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
