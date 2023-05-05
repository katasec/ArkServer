using Ark.Server.ExtensionMethods;
using Pulumi.AzureNative.Logic.Outputs;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Ark.Server;

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
        //builder.Host.UseSerilog();

        // Register Services
        builder.Services.RegisterServices();

        // Build App
        App = builder.Build();

        // Configure Middleware pipelines
        App.ConfigureMiddlewarePipeline();


    }



    public void Start()
    {
        CheckConfig();
        App.Run();
    }

    public void CheckConfig()
    {
        var config = Config.Read();
        var validator = new ConfigValidator();

        Console.WriteLine("Validating Config\n");
        validator.Validate(config);
    }
}

