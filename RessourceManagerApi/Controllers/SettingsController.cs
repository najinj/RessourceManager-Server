using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Core.ViewModels.Settings;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "User")]
        public async Task<BackOfficeSettingsViewModel> GetSettings() {
            var settings = await _backOfficeSettingsService.Get();
            return new BackOfficeSettingsViewModel(settings);
        }


        // POST: api/Settings
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<BackOfficeSettings> AddSettings(BackOfficeSettings settingsIn)
        {
            return await _backOfficeSettingsService.Create(settingsIn);
        }

        // PUT: api/Settings/5
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<BackOfficeSettingsViewModel> UpdateSettings(BackOfficeSettings settingsIn)
        {
            if (ModelState.IsValid)
            {
                var settings =  await _backOfficeSettingsService.Update(settingsIn);
                return new BackOfficeSettingsViewModel(settings);
            }
            return null;
        }
    }
}
