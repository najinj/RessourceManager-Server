
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using test_mongo_auth.Models;
using test_mongo_auth.Services;

namespace test_mongo_auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = false;
            });

            services.Configure<BookstoreDatabaseSettings>(
                      Configuration.GetSection(nameof(BookstoreDatabaseSettings)));

            services.AddSingleton<IBookstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<BookstoreDatabaseSettings>>().Value);

            var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = "RessourceManagmentDb"
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



            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            services.AddAuthentication(options =>
            {
                //Set default Authentication Schema as Bearer
                options.DefaultAuthenticateScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidIssuer = Configuration["JwtIssuer"],
                           ValidAudience = Configuration["JwtIssuer"],
                           IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                           ClockSkew = TimeSpan.Zero // remove delay of token when expire
                       };
            });




            services.AddSingleton<PostService>();
            services.AddSingleton<AreaService>();
            services.AddSingleton<AssetService>();
            services.AddSingleton<RessourceTypeService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            

            /*  services.AddIdentity<ApplicationUser, ApplicationRole>()
                          .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
                          (
                              "mongodb://localhost:27017",
                              "BookstoreDb"
                          )
                          .AddDefaultTokenProviders();
              var mongoDbIdentityConfiguration = new MongoDbIdentityConfiguration
              {
                  MongoDbSettings = new MongoDbSettings
                  {
                      ConnectionString = "mongodb://localhost:27017",
                      DatabaseName = "BookstoreDb"
                  },
                  IdentityOptionsAction = options =>
                  {
                      options.Password.RequireDigit = false;
                      options.Password.RequiredLength = 8;
                      options.Password.RequireNonAlphanumeric = false;
                      options.Password.RequireUppercase = false;
                      options.Password.RequireLowercase = false;

                      // Lockout settings
                      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                      options.Lockout.MaxFailedAccessAttempts = 10;

                      // ApplicationUser settings
                      options.User.RequireUniqueEmail = true;
                      options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
                  }
              };
              JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

              services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityConfiguration);

              services.AddAuthentication(options =>
              {
                  //Set default Authentication Schema as Bearer
                  options.DefaultAuthenticateScheme =
                             JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultScheme =
                             JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme =
                             JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(cfg =>
              {
                  cfg.RequireHttpsMetadata = false;
                  cfg.SaveToken = true;
                  cfg.TokenValidationParameters =
                         new TokenValidationParameters
                         {
                             ValidIssuer = Configuration["JwtIssuer"],
                             ValidAudience = Configuration["JwtIssuer"],
                             IssuerSigningKey =
                          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                             ClockSkew = TimeSpan.Zero // remove delay of token when expire
                         };
              });
              */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            CreateUserRoles(services).Wait();
        }



        private async Task CreateUserRoles(IServiceProvider serviceProvider)
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
            ApplicationUser user = await UserManager.FindByEmailAsync("naji@naji.com");
            var User = new ApplicationUser();
            await UserManager.AddToRoleAsync(user, "Admin");
        }
    }
}
