using Ark.Base;
using Ark.Server.Entities;
using Ark.ServiceModel.Cloudspace;
using Azure.Messaging.ServiceBus;
using Katasec.PulumiRunner;
using System.Text.Json;


//var x = new RemoteProgramArgs("aa", "aa", "aa");
//x.PulumiUp();

var c = Config.Read();

string connectionString = c.AzureConfig.MqConfig.MqConnectionString;
string queueName = c.AzureConfig.MqConfig.MqName;

Console.WriteLine($"Connection string:{connectionString}");
Console.WriteLine($"Queue Name:{queueName}");

await using var client = new ServiceBusClient(connectionString);

ServiceBusReceiver receiver = client.CreateReceiver(queueName);

Console.WriteLine("Listening");

while (true)
{
    // receive a message
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    // get the message body as a string
    string body = receivedMessage.Body.ToString();

    Console.WriteLine($"Subject: {receivedMessage.Subject}");
    Console.WriteLine($"Received: {body}");

    var x = JsonSerializer.Deserialize<AzureCloudspace>(body);
    Console.WriteLine($"Hub Name: {x.Hub.Name}");
    Console.WriteLine($"Name: {x.Hub.AddressPrefix}");
    // complete the message. messages is deleted from the queue. 
    await receiver.CompleteMessageAsync(receivedMessage);
}