using ArkServer.Entities.Azure;
using ArkServer.Features.Cloudspace;
using ArkServer.Repositories;
using ArkServer.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ArkServer.ExtensionMethods
{
    public static class ServiceRegistration
    {
        /// <summary>
        /// RegisterServices Add services to the DI container. 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Configure controllers and swagger
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Adding custom services
            services.AddSingleton<AsbService>();
            services.AddSingleton<Ark>();
            services.AddSingleton<IArkRepo, ArkJsonRepo>();
            services.AddSingleton<ArkService>();
            services.AddSingleton<AzureCloudspaceService>();

            // Model Validators
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<AzureCloudspaceRequest>, CloudspaceRequestValidator>();

            return services;
        }
    }
}
