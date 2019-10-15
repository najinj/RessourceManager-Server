using RessourceManager.Core.Models.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services.Interfaces
{
    public interface IEmailSettingService
    {
        Task<EmailSettings> Get();
        Task<EmailSettings> Create(EmailSettings emailSettingsIn);
        void Update(EmailSettings emailSettingsIn);
        void Remove();
    }
}
