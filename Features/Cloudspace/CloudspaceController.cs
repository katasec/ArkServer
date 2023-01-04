using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace ArkServer.Features.Cloudspace
{
    [ApiController]
    public class CloudspaceController : ControllerBase
    {
        private readonly AsbService _asbService;
        
        public CloudspaceController(AsbService asbService)
        {
            _asbService = asbService;
        }

        [HttpPost]
        [Route("/azure/cloudspace")]
        public async Task<IResult> Post(CloudspaceRequest req)
        {

            // Create message
            var message = new ServiceBusMessage(req.ToString()) { Subject = "CloudSpaceRequest" };

            // Send message
            await _asbService.Sender.SendMessageAsync(message);

            return Results.Accepted("/blah");
        }
    }
}
