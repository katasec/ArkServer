using ArkServer.Features.Cloudspace;
using ArkServer.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ArkServer.ExtensionMethods
{
    public static class ServiceCollection
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
            services.AddSingleton<IAzureCsRepo, CloudspaceJsonRepository>();
            
            // Model Validators
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CloudspaceRequest>, CloudspaceRequestValidator>();

            return services;
        }
    }
}
