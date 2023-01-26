using ArkServer.ExtensionMethods;
using Serilog;

namespace ArkServer;

public class Server
{
    public WebApplication App;

    public Server(string[] args)
    {

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        // Create App Builder and Register Services
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();

        
        // Register Services
        builder.Services.RegisterServices();

        // Build App
        App = builder.Build();

        // Configure Middleware pipelines
        App.ConfigureMiddlewarePipeline();

    }



    public void Start()
    {
        App.Run();
    }
}

