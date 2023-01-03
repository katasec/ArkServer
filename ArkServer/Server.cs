using ArkServer.ExtensionMethods;
using ArkServer.Handlers;


namespace ArkServer;

public class Server
{
    public WebApplication App;

    public Server(string[] args)
    {
        // Create App Builder and Register Services
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterServices();

        // Build App
        App = builder.Build();

        // Configure Middleware pipelines
        App.ConfigureMiddlewarePipeline();

        // Register Routes
        RegisterRoutes();
    }

    public void RegisterRoutes()
    {
        
        App.Get(HandlerFunc.WeatherHandler, "/weatherforecast");
        App.Get(HandlerFunc.HelloHandler, "/hello");

    }

    public void Start()
    {
        App.Run();
    }
}

