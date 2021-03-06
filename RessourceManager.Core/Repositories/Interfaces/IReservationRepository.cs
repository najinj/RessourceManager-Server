﻿using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RessourceManager.Core.Repositories.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<List<Reservation>> Get(DateTime start);
        Task<List<Reservation>> GetReservationsByResource(string resourceId , DateTime? startDate);
        Task<List<Reservation>> GetReservationsByInterval(DateTime start, DateTime end);
        Task Add(IEnumerable<Reservation> reservationsIn);
        Task RemovePeriodicReservations(string periodicId);
        Task<bool> CheckResourceAvailability(DateTime start, DateTime end, string spaceId);
        Task<List<Reservation>> GetPeriodicReservations(string periodicId);
        Task<List<Reservation>> GetUserReservations(string userId);
    }
}
