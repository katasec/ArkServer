using Azure.Messaging.ServiceBus;

namespace ArkServer.Services;

public class AsbService
{

    /// <summary>
    /// Sender sends messages to the ASB queue defined in ~/.ark/config
    /// </summary>
    public ServiceBusSender Sender { get; set; }

    /// <summary>
    /// Receiver sends messages to the ASB queue defined in ~/.ark/config
    /// </summary>
    public ServiceBusReceiver Receiver { get; set; }

    public AsbService()
    {
        // Read Ark Config
        var config = ArkConfig.Read();

        // Create ASB client from credentials in config
        var client = new ServiceBusClient(config.AzureConfig.MqConfig.MqConnectionString, new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        });

        // Initialize Sender and Receiver
        Sender = client.CreateSender(config.AzureConfig.MqConfig.MqName);
        Receiver = client.CreateReceiver(config.AzureConfig.MqConfig.MqName);
    }
}
