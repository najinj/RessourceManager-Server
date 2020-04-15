using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IBackOfficeSettingsService _backOfficeSettingsService;

        public SettingsController(IBackOfficeSettingsService backOfficeSettingsService)
        {
            _backOfficeSettingsService = backOfficeSettingsService;
        }
        // GET: api/Settings
        [HttpGet]
        public async Task<BackOfficeSettings> GetSettings() => await _backOfficeSettingsService.Get();

        // POST: api/Settings
        [HttpPost]
        public async Task<BackOfficeSettings> AddSettings(BackOfficeSettings settingsIn)
        {
            return await _backOfficeSettingsService.Create(settingsIn);
        }

        // PUT: api/Settings/5
        [HttpPut("{id}")]
        public async Task UpdateSettings(string id, BackOfficeSettings settingsIn)
        {
            await _backOfficeSettingsService.Update(settingsIn);
        }
    }
}
