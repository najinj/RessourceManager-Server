using System;
using System.Linq;
using System.Threading.Tasks;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;

namespace RessourceManager.Core.Services
{
    public class EmailSettingService : IEmailSettingService
    {
        private readonly IEmailSettingRepository _emailSettingRepository;

        public EmailSettingService(IEmailSettingRepository emailSettingRepository)
        {
            _emailSettingRepository = emailSettingRepository;
        }

        public async Task<EmailSettings> Create(EmailSettings emailSettingsIn)
        {
            try
            {
                await _emailSettingRepository.Add(emailSettingsIn);
                return emailSettingsIn;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<EmailSettings> Get()
        {
            var emailSettingsList = await _emailSettingRepository.GetAll();
            return emailSettingsList.FirstOrDefault();       
        }

        public async void Remove()
        {
            var emailSettings = await Get();
            await _emailSettingRepository.Remove(emailSettings.Id);
        }

        public async void Update(EmailSettings emailSettingsIn)
        {
            await _emailSettingRepository.Update(emailSettingsIn);
        }
    }
}
