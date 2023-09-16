using Ark.Base;
using Azure.Messaging.ServiceBus;
using Serilog;
using YamlDotNet.Serialization;
using System.Text.Json;
using Katasec.PulumiRunner;
using static Katasec.PulumiRunner.RemoteProgram;

namespace Ark.Worker;

public class Worker
{
    ILogger _logger;
    string connectionString;
    string queueName;
    Config c;
    ServiceBusClient _client;
    ServiceBusReceiver _receiver;
    private static readonly string gitUrl = "https://github.com/katasec/library";

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
            try
            {
                // receive a message
                _logger.Information("Waiting for messages...");
                _receiver = _client.CreateReceiver(queueName, new ServiceBusReceiverOptions
                {
                    ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete  
                });
                ServiceBusReceivedMessage receivedMessage = await _receiver.ReceiveMessageAsync();

                // Skip null messages
                if (receivedMessage == null)
                {
                    _logger.Information("Received null message, contnuing...");
                    continue;
                }

                // Handle message
                _logger.Information($"Handling message with subject: {receivedMessage.Subject}");
                await Handle(receivedMessage.Subject, receivedMessage.Body.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }
    }
    public async Task Handle(string subject, string body)
    {
        // Log subject
        _logger.Information($"Subject: {subject}");
        var action = subject.Split(';')[0];
        var messageType = subject.Split(';')[1];

        // Convert message to entity of  type "T" retrieved from the subject
        var T = Type.GetType($"{messageType}, Ark.Entities");
        if ( T == null)
        {
            _logger.Error($"Could not serialize message with type: {messageType}");
            return;
        }
        _logger.Information($"Serializing message Type {T.FullName}");

        // Determnine handler name from Entity Type
        var handlername = $"{T.Name.ToLower()}-handler";
        _logger.Information($"Handler name: {handlername}");

        try
        {
            // Create resource of Type "T"
            var resource = JsonSerializer.Deserialize(body, T);

            // Generate yaml config for Pulumi.yaml for resource
            var ser = new SerializerBuilder().Build();
            var arkdata = ser.Serialize(resource);

            // Inject yaml config into configfile
            var pulumiProgram = new RemoteProgram(
                stackName: "dev",
                gitUrl: gitUrl,
                projectPath: handlername,
                plugins: new List<Plugin> { 
                    new Plugin("azure-native", "2.6.0"),
                }
            );
            pulumiProgram.InjectArkData(arkdata);
            switch (action)
            {
                case "create":
                    await pulumiProgram.Up();
                    break;
                case "delete":
                    await pulumiProgram.Destroy();
                    break;
                default:
                    _logger.Error($"Unknown action: {action}");
                    break;
            }

            _logger.Information($"Finished handling message with subject: {subject}");
        }
        catch (Exception ex)
        {
            _logger.Information($"Failed to deserialize {ex.Message}");
        }
    }

    public void Cleanup(object? sender, EventArgs args)
    {
        
        _logger.Information("Shutting down...");
    }
}
