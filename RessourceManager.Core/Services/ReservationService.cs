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
        private readonly IAssetRepository _assetRepository;
        private readonly ISpaceRepository _spaceRepository;
        private readonly IErrorHandler _errorHandler;
        public ReservationService(IReservationRepository reservationRepository, 
                                  IAssetRepository assetRepository,
                                  IErrorHandler errorHandler,
                                  ISpaceRepository spaceRepository)
        {
            _reservationRepository = reservationRepository;
            _errorHandler = errorHandler;
            _assetRepository = assetRepository;
            _spaceRepository = spaceRepository;
            _errorHandler = errorHandler;
        }

        public async Task<Reservation> Add(Reservation reservationIn)
        {
            var availability = await _reservationRepository.CheckResourceAvailability(reservationIn.Start, reservationIn.End, reservationIn.ResourceId);
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
                var availability = await _reservationRepository.CheckResourceAvailability(reservation.Start, reservation.End, reservation.ResourceId);
                if(!availability)
                    throw new ReservationServiceException(_errorHandler.GetMessage(ErrorMessagesEnum.NotAvailable)
                       , new string[] { nameof(Reservation.Start), nameof(Reservation.End) });
            }
            await _reservationRepository.Add(reservationsIn);
            return reservationsIn;
        }

        public async Task<dynamic> Availability(DateTime start, DateTime end, RType resourceType)
        {
            var reservations = await _reservationRepository.GetReservationsByInterval(start, end);
            var resourceIds = reservations.Where(reservation => reservation.ResourceType == resourceType).Select(reservation=> reservation.ResourceId).ToList();
            if(resourceType == RType.Space)
            {
                var spaces = await _spaceRepository.GetAll();
                var freeSpaces = spaces.Where(space => !resourceIds.Contains(space.Id)).ToList();
                return freeSpaces;
            }
            else
            {
                var assets = await _assetRepository.GetAll();
                var freeAssets = assets.Where(asset => !resourceIds.Contains(asset.Id)).ToList();
                return freeAssets;
            }
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

        public async Task<List<Reservation>> GetPeriodicReservations(string periodicId)
        {
            var reservations = await _reservationRepository.GetPeriodicReservations(periodicId);
            return reservations;
        }

        public async Task<List<Reservation>> GetReservationsByResource(string resourceId, DateTime? startDate)
        {
            var reservations = await _reservationRepository.GetReservationsByResource(resourceId, startDate);
            return reservations;
        }

        public async Task Remove(string reservationId,string userId,bool isAdmin)
        {
            var reservation = await Get(reservationId);
            if (reservation.UserId != userId && !isAdmin)
                throw new ReservationServiceException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotDelete),nameof(Reservation)),
                                new string[] { nameof(Reservation.UserId) });
            await _reservationRepository.Remove(reservationId);
        }

        public async Task RemovePeriodicReservations(string periodicId, string userId, bool isAdmin)
        {
            var reservations = await _reservationRepository.GetPeriodicReservations(periodicId);
            var reservationUserId = reservations.FirstOrDefault().UserId;
            if(reservationUserId != userId && !isAdmin)
                throw new ReservationServiceException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.AuthCannotDelete), nameof(Reservation)),
                                new string[] { nameof(Reservation.UserId) });
            await _reservationRepository.RemovePeriodicReservations(periodicId);
        }

        public Task Update(Reservation reservationIn)
        {
            throw new System.NotImplementedException();
        }
    }
}
