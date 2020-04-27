
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RessourceManager.Core.Models.V1;
using RessourceManager.Api.Infrastructure.Middlewares;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Extensions;
using RessourceManagerApi.ExtensionMethods;
using Microsoft.OpenApi.Models;
using RessourceManagerApi.Infrastructure;
using RessourceManager.Infrastructure.DatabaseSettings;

namespace RessourceManagerApi
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = false;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
            
            Installer.ConfigureServices(services, Configuration);

            Installer.ConfigureMongoDbIdentity(services);

            services.AddMvc();

            RegisterAuth(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IServiceProvider serviceProvider)
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

            app.UseCors("MyPolicy");

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            ConfigureAuth(app);

            app.UseHttpsRedirection();     
            
            app.UseTokenProvider(_tokenProviderOptions);

            app.UseAuthentication();       
            
            app.UseMvc();

            Installer.CreateUserRoles(serviceProvider).Wait();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ResourceManager API v1");
            });
        }       
    }
}
