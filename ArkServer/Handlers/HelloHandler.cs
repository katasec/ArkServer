using Azure.Messaging.ServiceBus;
using System.Data.SqlTypes;
using YamlDotNet.Serialization;
namespace ArkServer.Handlers;

public static partial class HandlerFunc
{
    public static Func<string> HelloHandler =  () =>
    {
        var deserializer = new DeserializerBuilder().Build();
        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var arkConfigFile = Path.Join(homeFolder, ".ark", "config");

        var config = File.ReadAllText(arkConfigFile);

        var c = deserializer.Deserialize<Config>(config);

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        
        var client = new ServiceBusClient(c.azureconfig.mqconfig.mqconnectionstring, clientOptions);
        var sender = client.CreateSender(c.azureconfig.mqconfig.mqname);

        var message = new ServiceBusMessage("Hello from Ark");
        sender.SendMessageAsync(message).Wait();
        return $"Hello {c.azureconfig.mqconfig.mqconnectionstring}";
    };
}

public struct Config
{
    public string cloudid { get; set; }
    public AzureConfig azureconfig { get; set; }

    public AwsConfig awsconfig { get; set; }

    public string logfile { get; set; }
}

public struct AwsConfig
{
    public StorageConfig storageconfig { get; set; }
    public MqConfig mqconfig { get; set; }
}

public struct AzureConfig
{
    public string resourcegroupname { get; set; }
    public StorageConfig storageconfig { get; set; }
    public MqConfig mqconfig{ get; set; }

}

public struct StorageConfig
{
    public string logstorageaccountname { get; set; }
    public string logstorageendpoint { get; set; }
    public string logscontainer { get; set; }
    public string logstoragekey { get; set; }
    public string pulumistatecontainer { get; set; }

}
public struct MqConfig
{
    public string mqconnectionstring { get; set; }
    public string mqname { get; set; }
}