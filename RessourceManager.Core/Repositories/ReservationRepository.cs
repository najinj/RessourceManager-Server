using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;

namespace RessourceManager.Core.Repositories
{
    public class ReservationRepository : Repository<Reservation> , IReservationRepository
    {
        public ReservationRepository(IMongoContext context) : base(context)
        {

        }
    }
}
