
using RessourceManager.Core.Models.V1;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IReservationSettingsService
    {
        Task<ReservationSettings> Get();
        Task<ReservationSettings> Create(ReservationSettings reservationSettingsIn);
        void Update(ReservationSettings reservationSettingsIn);
        void Remove();
    }
}
