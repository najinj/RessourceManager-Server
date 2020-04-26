using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RessourceManager.Infrastructure.DatabaseSettings;


namespace RessourceManager.Infrastructure
{
    public static class Installer
    {
        public static void RegisterServices(IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<RessourceDatabaseSettings>(x=>
                      configuration.GetSection(nameof(RessourceDatabaseSettings)).Bind(x));
            
            services.AddSingleton<IRessourceDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<RessourceDatabaseSettings>>().Value);

     
        }
    }
}
