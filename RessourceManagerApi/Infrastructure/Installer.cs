

using Microsoft.Extensions.DependencyInjection;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Infrastructure.Context;

namespace RessourceManagerApi.Infrastructure
{
    internal static class Installer
    {

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMongoContext, MongoContext>();

            services.AddTransient<IRessourceTypeRepository, RessourceTypeRepository>();
            services.AddTransient<IRessourceTypeService, RessourceTypeService>();

            services.AddTransient<ISpaceRepository, SpaceRepository>();
            services.AddTransient<ISpaceService, SpaceService>();

            services.AddTransient<IAssetRepository, AssetRepository>();
            services.AddTransient<IAssetService, AssetService>();

            services.AddTransient<IEmailSettingRepository, EmailSettingRepository>();
            services.AddTransient<IEmailSettingService, EmailSettingService>();

            services.AddTransient<IEmailSenderService, EmailSenderService>();

            services.AddTransient<IErrorHandler, ErrorHandler>();
        }

    }
}
