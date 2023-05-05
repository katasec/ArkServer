namespace Ark.Server.ExtensionMethods
{
    public static class MiddlewarePipeline
    {
        /// <summary>
        /// ConfigureMiddlewarePipeline registers the sequence of middleware to use for our web application
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
        {
            app.MapControllers();
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
