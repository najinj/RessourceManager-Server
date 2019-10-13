

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
            services.AddTransient<IErrorHandler, ErrorHandler>();
        }

    }
}
