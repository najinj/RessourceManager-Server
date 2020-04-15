using System;
using System.Linq;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;


namespace RessourceManager.Core.Services.Interfaces
{
    public class BackOfficeSettingsService : IBackOfficeSettingsService
    {
        private readonly IBackOfficeSettingsRepository _backOfficeSettingsRepository;

        public BackOfficeSettingsService(IBackOfficeSettingsRepository backOfficeSettingsRepository)
        {
            _backOfficeSettingsRepository = backOfficeSettingsRepository;
        }

        public async Task<BackOfficeSettings> Create(BackOfficeSettings calendarSettingsIn)
        {
            try
            {
                await _backOfficeSettingsRepository.Add(calendarSettingsIn);
                return calendarSettingsIn;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<BackOfficeSettings> Get()
        {
            var calendarSettingsList = await _backOfficeSettingsRepository.GetAll();
            return calendarSettingsList.FirstOrDefault();
        }

        public async Task Remove()
        {
            var settings = await Get();
            await _backOfficeSettingsRepository.Remove(settings.Id);
        }

        public async Task Update(BackOfficeSettings settings)
        {
            await _backOfficeSettingsRepository.Update(settings);
        }
    }
}
