using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArkServer.Features.Cloudspace
{
    [ApiController]
    public class CloudspaceController : ControllerBase
    {
        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(CloudspaceRequest req)
        {
            // Read Ark ArkConfig
            var config = ArkConfig.Read();

            // Create service bus client
            var client = new ServiceBusClient(config.AzureConfig.MqConfig.MqConnectionString, new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });

            // Create service bus sender
            var sender = client.CreateSender(config.AzureConfig.MqConfig.MqName);

            // Send message
            var message = new ServiceBusMessage("Hello from Ark")
            {
                Subject = "CloudSpaceRequest"
            };
            await sender.SendMessageAsync(message);

            Console.WriteLine(req.ProjectName);
            return Results.Ok("Everything is OK");
        }
    }
}
