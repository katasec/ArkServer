
namespace ArkServer.ExtensionMethods
{
    public static class HttpPipeline
    {
        /// <summary>
        /// ConfigureMiddlewarePipeline registers the sequence of middleware to use for our web application
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
        {
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI();

            

            //app.UseReDoc(options =>
            //{
            //    options.DocumentTitle = "Swagger Demo Documentation";
            //    options.SpecUrl = "/swagger/v1/swagger.json";
            //});


            
            

            return app;
        }
    }
}
