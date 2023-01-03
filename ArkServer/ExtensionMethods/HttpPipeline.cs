
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
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            return app;
        }
    }
}
