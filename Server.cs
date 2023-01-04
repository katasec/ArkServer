using ArkServer.ExtensionMethods;
using ArkServer.Services;

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
    }



    public void Start()
    {
        App.Run();
    }
}

