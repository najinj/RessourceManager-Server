using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;

namespace RessourceManager.Core.Repositories
{
    public class CalendarSettingsRepository : Repository<CalendarSettings>, ICalendarSettingsRepository
    {
        public CalendarSettingsRepository(IMongoContext context) : base(context)
        {

        }
    }
}
