using Ark.Base;
using Azure.Messaging.ServiceBus;
using Serilog;
using YamlDotNet.Serialization;
using System.Text.Json;
using Ark.Entities;
using static ServiceStack.Diagnostics.Events;

namespace Ark.Worker;

public class Worker
{
    ILogger _logger;
    string connectionString;
    string queueName;
    Config c;
    ServiceBusClient _client;
    ServiceBusReceiver _receiver;

    public Worker()
    {
        _logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();
     
        // Get MQ client using creds in Ark config
        c = Config.Read();
        queueName = c.AzureConfig.MqConfig.MqName;
        connectionString = c.AzureConfig.MqConfig.MqConnectionString;
        _client = new ServiceBusClient(connectionString);

        // Initalize mq receiver
        _receiver = _client.CreateReceiver(queueName);
    }

    public async Task Start()
    {

        _logger.Information("Starting Worker...");
        

        // Capture application exits and run cleanup
        Console.CancelKeyPress += new ConsoleCancelEventHandler(Cleanup);
        AppDomain.CurrentDomain.ProcessExit += Cleanup;

        _logger.Information($"Local ark server: http://{c.ApiServer.Host}:{c.ApiServer.Port}");

        while (true)
        {
            // receive a message
            _logger.Information("Listening for messages...");
            ServiceBusReceivedMessage receivedMessage = await _receiver.ReceiveMessageAsync();

            // Handle Message
            await Handle(receivedMessage);

        }
    }
    public async Task Handle(ServiceBusReceivedMessage message)
    {
        if (message == null)
        {
            _logger.Information("Skipping null message");
            return;
        }

        // Log subject
        string body = message.Body.ToString();
        _logger.Information($"Subject: {message.Subject}");

        // Convert message to AzureCloudspace object
        var T = Type.GetType($"{message.Subject}, Ark.Entities");

        if ( T == null)
        {
            return;
        }

        _logger.Information($"T was not null :) !! {T.FullName}");
        try
        {
            var resource = JsonSerializer.Deserialize(body, T);

            // Generate yaml config for Pulumi.yaml for resource

            var ser = new SerializerBuilder().Build();
            var arkdata = ser.Serialize(resource);

            _logger.Information(arkdata);
            // Inject yaml config into configfile
            //p.InjectArkData(arkdata);

        }
        catch (Exception ex)
        {
            _logger.Information($"Failed to deserialize {ex.Message}");
        }
        // complete the message. messages is deleted from the queue. 
        await _receiver.CompleteMessageAsync(message);
    }

    public void Cleanup(object? sender, EventArgs args)
    {
        
        _logger.Information("Shutting down...");
    }
}
