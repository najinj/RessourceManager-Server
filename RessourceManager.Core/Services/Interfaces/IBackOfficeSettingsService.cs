using RessourceManager.Core.Models.V1;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IBackOfficeSettingsService
    {
        Task<BackOfficeSettings> Get();
        Task<BackOfficeSettings> Create(BackOfficeSettings settingsIn);
        Task Update(BackOfficeSettings settingsIn);
        Task Remove();
    }
}
