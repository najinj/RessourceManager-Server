using System;
using System.Linq;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;


namespace RessourceManager.Core.Services.Interfaces
{
    public class CalendarSettingsService
    {
        private readonly ICalendarSettingsRepository _calendarSettingRepository;

        public CalendarSettingsService(CalendarSettingsRepository calendarSettingRepository)
        {
            _calendarSettingRepository = calendarSettingRepository;
        }

        public async Task<CalendarSettings> Create(CalendarSettings calendarSettingsIn)
        {
            try
            {
                await _calendarSettingRepository.Add(calendarSettingsIn);
                return calendarSettingsIn;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<CalendarSettings> Get()
        {
            var calendarSettingsList = await _calendarSettingRepository.GetAll();
            return calendarSettingsList.FirstOrDefault();
        }

        public async void Remove()
        {
            var calendarSettings = await Get();
            await _calendarSettingRepository.Remove(calendarSettings.Id);
        }

        public async void Update(CalendarSettings calendarSettingsIn)
        {
            await _calendarSettingRepository.Update(calendarSettingsIn);
        }
    }
}
