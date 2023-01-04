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
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
