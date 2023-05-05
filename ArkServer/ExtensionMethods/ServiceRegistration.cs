using ArkServer.Entities;
using ArkServer.Features.Cloudspace;
using ArkServer.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using ServiceStack.OrmLite;
using Katasec.AspNet.YamlFormatter;
using Ark.ServiceModel.Cloudspace;

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
            services.AddControllers(x =>
            {
                x.InputFormatters.Clear();
                x.InputFormatters.Add(new YamlInputFormatter());
                x.OutputFormatters.Add(new YamlOutputFormatter());
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Adding custom services
            services.AddSingleton<AsbService>();

            // Model Validators
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreateAzureCloudspaceRequest>, CreateAzureCloudspaceValidator>();

            // Initalize OrmLite
            var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var dbFile = Path.Join(homeDir, ".ark", "db", "ark.db");
            var  dbFactory = new OrmLiteConnectionFactory(dbFile, SqliteDialect.Provider);
            services.AddSingleton(dbFactory);

            

            // Create Tables if not exist
            var db = dbFactory.Open();
            db.CreateTableIfNotExists<AzureCloudspace>();
            db.CreateTableIfNotExists<HelloSuccess>();


            return services;
        }
    }
}
