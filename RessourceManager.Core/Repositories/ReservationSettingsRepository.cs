using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;


namespace RessourceManager.Core.Repositories
{
    public class ReservationSettingsRepository : Repository<ReservationSettings>, IReservationSettingsRepository
    {
        public ReservationSettingsRepository(IMongoContext context) : base(context)
        {
        }
    }
}
