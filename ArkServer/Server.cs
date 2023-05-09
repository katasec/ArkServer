using Ark.Server.ExtensionMethods;
using Pulumi.AzureNative.Logic.Outputs;
using Serilog;
using ServiceStack;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using ServiceStack.Script;
using Ark.Base;
using Serilog.Events;

namespace Ark.Server;

public class Server
{
    public WebApplication App;

    public Server(string[] args)
    {

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.Console()
            .CreateLogger();

        // Create App Builder and Register Services
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();
        builder.WebHost.UseUrls(new string[] { "http://0.0.0.0:5067" });

        // Register Services
        builder.Services.RegisterServices();

        // Build App
        App = builder.Build();

        

        // Configure Middleware pipelines
        App.ConfigureMiddlewarePipeline();


    }

    public void CheckConfig()
    {

        var config = Config.Read();
        var validator = new ConfigValidator();

        var results = validator.Validate(config);

        if (!results.IsValid)
        {
            Console.WriteLine("Config errors:");
            foreach (var result in results.InList())
            {
                Console.WriteLine(result.ToString("\n"));
            }
            Environment.Exit(0);
        }
    }


    public void Start()
    {
        CheckConfig();
        App.Run();
    }


}

