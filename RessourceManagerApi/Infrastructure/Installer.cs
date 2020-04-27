

using System;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services;
using RessourceManager.Core.Services.Interfaces;
using RessourceManager.Infrastructure.Context;
using RessourceManager.Infrastructure.DatabaseSettings;
using System.Threading.Tasks;

namespace RessourceManagerApi.Infrastructure
{
    internal static class Installer
    {

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RessourceDatabaseSettings>(x =>
                         configuration.GetSection(nameof(RessourceDatabaseSettings)).Bind(x));

            services.AddSingleton<IRessourceDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<RessourceDatabaseSettings>>().Value);




            services.AddTransient<IMongoContext, MongoContext>();

            services.AddTransient<IRessourceTypeRepository, RessourceTypeRepository>();
            services.AddTransient<IRessourceTypeService, RessourceTypeService>();

            services.AddTransient<ISpaceRepository, SpaceRepository>();
            services.AddTransient<ISpaceService, SpaceService>();

            services.AddTransient<IAssetRepository, AssetRepository>();
            services.AddTransient<IAssetService, AssetService>();

            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IReservationService, ReservationService>();

            services.AddTransient<IBackOfficeSettingsRepository, BackOfficeSettingsRepository>();
            services.AddTransient<IBackOfficeSettingsService, BackOfficeSettingsService>();

            services.AddTransient<IEmailSenderService, EmailSenderService>();

            services.AddTransient<IErrorHandler, ErrorHandler>();
        }

        public static void ConfigureMongoDbIdentity(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var ressourceDatabaseSettingsService = serviceProvider.GetService<IRessourceDatabaseSettings>();

            var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = ressourceDatabaseSettingsService.ConnectionString,
                    DatabaseName = ressourceDatabaseSettingsService.DatabaseName
                },
                IdentityOptionsAction = options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;

                    // ApplicationUser settings
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
                }
            };

            services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfiguration);
        }

        public static async Task CreateUsersAndRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new ApplicationRole("Admin"));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            ApplicationUser admin = await UserManager.FindByEmailAsync("admin2@admin.com");
            if (admin != null)
                await UserManager.AddToRoleAsync(admin, "Admin");
            else
            {
                 admin = new ApplicationUser
                 {
                     Name = "Admin",
                     LastName = "Admin",
                     City = "",
                     UserName = "admin2@admin.com",
                     Email = "admin2@admin.com"
                 };
                var result = await UserManager.CreateAsync(admin, "Admin@2020");
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

    }
}
