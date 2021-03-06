﻿using System.Linq;
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
        private readonly IBackOfficeSettingsService _backOfficeSettingsService;
        public ReservationService(IReservationRepository reservationRepository, 
                                  IAssetRepository assetRepository,
                                  IErrorHandler errorHandler,
                                  ISpaceRepository spaceRepository,
                                  IBackOfficeSettingsService backOfficeSettingsService)
        {
            _reservationRepository = reservationRepository;
            _errorHandler = errorHandler;
            _assetRepository = assetRepository;
            _spaceRepository = spaceRepository;
            _errorHandler = errorHandler;
            _backOfficeSettingsService = backOfficeSettingsService;
        }

        public async Task<Reservation> Add(Reservation reservationIn)
        {
            var validDuration = await ValidateReservationDuration(reservationIn);
            if (!validDuration)
            {
                    throw new ReservationServiceException(_errorHandler.GetMessage(ErrorMessagesEnum.MaximumDurationExceeded)
                            , new string[] { nameof(Reservation.End) });
            }

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
                var validDuration = await ValidateReservationDuration(reservation);
                if (!validDuration)
                {
                    throw new ReservationServiceException(_errorHandler.GetMessage(ErrorMessagesEnum.MaximumDurationExceeded)
                            , new string[] { nameof(Reservation.End) });
                }
                var availability = await _reservationRepository.CheckResourceAvailability(reservation.Start, reservation.End, reservation.ResourceId);
                if(!availability)
                    throw new ReservationServiceException(_errorHandler.GetMessage(ErrorMessagesEnum.NotAvailable)
                       , new string[] { nameof(Reservation.Start), nameof(Reservation.End) });
            }
            await _reservationRepository.Add(reservationsIn);
            return reservationsIn;
        }

        public async Task<dynamic> Availability(DateTime start, DateTime end, RType resourceType,string[] resourceSubTypes,IEnumerable<DateTime> occurrences)
        {
            var reservations = new List<Reservation>();
            if(occurrences == null)
                reservations = await _reservationRepository.GetReservationsByInterval(start, end);
            else
            {
                foreach (var startTime in occurrences)
                {
                    var endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, end.Hour, end.Minute, 0);
                    var tempResources = await _reservationRepository.GetReservationsByInterval(startTime, endTime);
                    reservations.AddRange(tempResources);
                }
            }
            var resourceIds = reservations.Where(reservation => reservation.ResourceType == resourceType).Select(reservation=> reservation.ResourceId).ToList();
            if(resourceType == RType.Space)
            {
                var spaces = await _spaceRepository.GetAll();
                var freeSpaces = spaces.Where(space => !resourceIds.Contains(space.Id)).ToList();
                if (resourceSubTypes.Any() && freeSpaces.Any())
                    freeSpaces = freeSpaces.Where(space => resourceSubTypes.Contains(space.SpaceTypeId)).ToList();
                return freeSpaces;
            }
            else
            {
                var assets = await _assetRepository.GetAll();
                var freeAssets = assets.Where(asset => !resourceIds.Contains(asset.Id)).ToList();
                if (resourceSubTypes.Any() && freeAssets.Any())
                    freeAssets = freeAssets.Where(asset => resourceSubTypes.Contains(asset.AssetTypeId)).ToList();
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
        public async Task<List<Reservation>> GetUserReservations(string userId)
        {
            var reservations = await _reservationRepository.GetUserReservations(userId);
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

        private async Task<bool> ValidateReservationDuration(Reservation reservation)
        {
            var settings = await _backOfficeSettingsService.Get();
            var reservationSettings = settings.ReservationSettings;
            if (!string.IsNullOrEmpty(reservation.PeriodicId))
            {
                var diff = (reservation.EndTime.TimeOfDay - reservation.StartTime.TimeOfDay).TotalMinutes / 60;
                if (diff > reservationSettings.MaxDurationPerReservation)
                    return false;
                return true;
            }
            else
            {
                var diff = (reservation.End - reservation.Start).TotalMinutes / 60;
                if (diff > reservationSettings.MaxDurationPerReservation)
                    return false;
                return true;
            }
        }
    }
}
