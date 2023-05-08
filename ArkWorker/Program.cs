using Ark.Base;
using Ark.Server.Entities;
using Ark.ServiceModel.Cloudspace;
using Azure.Messaging.ServiceBus;
using Katasec.PulumiRunner;
using System.Text.Json;
using YamlDotNet;
using YamlDotNet.Serialization;

using Pulumi.Automation;

var p = new RemoteProgram("dev", "https://github.com/katasec/library", "azurecloudspace-handler");
//p.PulumiUp();

var c = Config.Read();

string connectionString = c.AzureConfig.MqConfig.MqConnectionString;
string queueName = c.AzureConfig.MqConfig.MqName;

await using var client = new ServiceBusClient(connectionString);

ServiceBusReceiver receiver = client.CreateReceiver(queueName);

Console.WriteLine("Listening");

while (true)
{
    // receive a message
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    if (receivedMessage !=null)
    {
        // Log subject
        string body = receivedMessage.Body.ToString();
        Console.WriteLine($"Subject: {receivedMessage.Subject}");

        // Convert message to AzureCloudspace object
        var acs = JsonSerializer.Deserialize<AzureCloudspace>(body);

        try
        {
            // Generate yaml config for Pulumi.yaml from AzureCloudspace object
            var ser = new SerializerBuilder().Build();
            var arkdata = ser.Serialize(acs);

            // Inject yaml config into configfile
            p.InjectArkData(arkdata);
            
        } catch (Exception ex)
        {
            Console.WriteLine($"Failed to deserialize {ex.Message}");
        }

        // complete the message. messages is deleted from the queue. 
        await receiver.CompleteMessageAsync(receivedMessage);
    } else
    {
        Console.WriteLine("Error, received null message");
    }
}