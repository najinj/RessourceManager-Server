using RessourceManager.Core.Models.V1;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    interface ICaneldarSettingsService
    {
        Task<CalendarSettings> Get();
        Task<CalendarSettings> Create(CalendarSettings emailSettingsIn);
        void Update(CalendarSettings emailSettingsIn);
        void Remove();
    }
}
