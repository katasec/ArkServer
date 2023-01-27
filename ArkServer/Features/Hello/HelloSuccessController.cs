using ArkServer.Services;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ArkServer.Entities.Azure;
using ServiceStack.Messaging;
using ServiceStack.OrmLite;

namespace ArkServer.Features;

[ApiController]
public class HelloSuccessController
{
    AsbService _asbService;
    private System.Data.IDbConnection _db;
    public HelloSuccessController(AsbService asbService,OrmLiteConnectionFactory dbFactory)
    {
        _asbService= asbService;
        _db = dbFactory.Open();
    }

    [HttpPost]
    [Route("/azure/hellosuccess")]
    public async Task<IResult> CreateHelloGood(HelloSuccessRequest req)
    {
        var hello = new HelloSuccess { Message= req.Message};
        
        await _asbService.Sender.SendMessageAsync(new ServiceBusMessage(hello.ToString()) { Subject = req.GetType().Name });
        return Results.Accepted("OK",new HelloSuccessResponse{Id = req.Id});
    }
}
