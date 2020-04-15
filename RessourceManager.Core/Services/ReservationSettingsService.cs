using System;
using System.Linq;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;

namespace RessourceManager.Core.Services
{
    public class ReservationSettingsService : IReservationSettingsService
    {
        private readonly IReservationSettingsRepository _reservationSettingRepository;

        public ReservationSettingsService(ReservationSettingsRepository reservationSettingRepository)
        {
            _reservationSettingRepository = reservationSettingRepository;
        }

        public async Task<ReservationSettings> Create(ReservationSettings reservationSettingsIn)
        {
            try
            {
                await _reservationSettingRepository.Add(reservationSettingsIn);
                return reservationSettingsIn;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<ReservationSettings> Get()
        {
            var reservationSettingsList = await _reservationSettingRepository.GetAll();
            return reservationSettingsList.FirstOrDefault();
        }

        public async void Remove()
        {
            var reservationSettings = await Get();
            await _reservationSettingRepository.Remove(reservationSettings.Id);
        }

        public async void Update(ReservationSettings reservationSettingsIn)
        {
            await _reservationSettingRepository.Update(reservationSettingsIn);
        }
    }
}
