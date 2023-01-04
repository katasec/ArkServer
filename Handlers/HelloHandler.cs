using Azure.Messaging.ServiceBus;
using System.Data.SqlTypes;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArkServer.Handlers;

public static partial class HandlerFunc
{
    public static Func<string> HelloHandler =  () =>
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var arkConfigFile = Path.Join(homeFolder, ".ark", "config");

        var config = File.ReadAllText(arkConfigFile);

        var c = deserializer.Deserialize<Config>(config);

        var clientOptions = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        
        var client = new ServiceBusClient(c.AzureConfig.MqConfig.MqConnectionString, clientOptions);
        var sender = client.CreateSender(c.AzureConfig.MqConfig.MqName);

        /*
        var message = new ServiceBusMessage("Hello from Ark");
        sender.SendMessageAsync(message).Wait();
        */

        return $"Hello {c.AzureConfig.MqConfig.MqConnectionString}";
    };
}

public struct Config
{
    [YamlMember(Alias = "cloudid")]
    public string CloudId { get; set; }

    [YamlMember(Alias = "azureconfig")]
    public AzureConfig AzureConfig { get; set; }

    [YamlMember(Alias = "awsconfig")]
    public AwsConfig AwsConfig { get; set; }

    [YamlMember(Alias = "logfile")]
    public string LogFile { get; set; }
}

public struct AwsConfig
{
    [YamlMember(Alias = "storageconfig")]
    public StorageConfig StorageConfig { get; set; }

    [YamlMember(Alias = "mqconfig")]
    public MqConfig MqConfig { get; set; }
}

public struct AzureConfig
{
    [YamlMember(Alias = "resourcegroupname")]
    public string ResourceGroupName { get; set; }

    [YamlMember(Alias = "storageconfig")]
    public StorageConfig StorageConfig { get; set; }

    [YamlMember(Alias = "mqconfig")]
    public MqConfig MqConfig{ get; set; }

}

public struct StorageConfig
{
    [YamlMember(Alias = "logstorageaccountname")]
    public string LogStorageAccountName { get; set; }

    [YamlMember(Alias = "logstorageendpoint")]
    public string LogStorageEndpoint { get; set; }

    [YamlMember(Alias = "logscontainer")]
    public string LogsContainer { get; set; }

    [YamlMember(Alias = "logstoragekey")]
    public string LogStorageKey { get; set; }

    [YamlMember(Alias = "pulumistatecontainer")]
    public string PulumiStateContainer { get; set; }

}
public struct MqConfig
{
    [YamlMember(Alias = "mqconnectionstring")]
    public string MqConnectionString { get; set; }

    [YamlMember(Alias = "mqname")]
    public string MqName { get; set; }
}