using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;

namespace RessourceManager.Core.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(IMongoContext context) : base(context)
        {

        }
        public async Task<bool> CheckAvailability(DateTime start, DateTime end, string spaceId)
        {
                var reservations = await GetReservationsByInterval(start, end);
                if(reservations.Exists(reservation=>reservation.ResourceId == spaceId))
                    return false;
                return true;              
        }

        public async Task<List<Reservation>> Get(DateTime start)
        {
            var reservations = await DbSet.FindAsync(reservation => reservation.Start >= start || reservation.End >= start);
            return reservations.ToList();
        }

        public async Task<List<Reservation>> GetReservationsByInterval(DateTime start, DateTime end)
        {
            var reservations = await DbSet.FindAsync(reservation => 

                (reservation.Start <= start && reservation.End > start) ||

                (start <= reservation.Start && end >= reservation.End) ||

                (reservation.Start < end && reservation.End >= end) ||

                (reservation.Start > start && reservation.End < end)
            );
            return reservations.ToList();
        }

        public async Task<List<Reservation>> GetReservationsByResource(string resourceId)
        {
            var reservations = await DbSet.FindAsync(reservation => reservation.ResourceId == resourceId);
            return reservations.ToList();
        }

        public async Task RemovePeriodicReservations(string periodicId)
        {
            var result = await DbSet.DeleteManyAsync(reservation => reservation.PeriodicId == periodicId);
        }


        public async Task Add(IEnumerable<Reservation> reservationsIn)
        {
             await DbSet.InsertManyAsync(reservationsIn);
        }
    }
}
