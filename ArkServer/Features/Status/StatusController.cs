using Microsoft.AspNetCore.Mvc;
using PulumiApi;
using static ServiceStack.Diagnostics.Events;
using System.Diagnostics;
using YamlDotNet.Core;
using System.Text.Json.Serialization;

namespace ArkServer.Features.Status;



[ApiController]
public class StatusController
{
    ApiClient _api;
	public StatusController()
	{
          _api= new ApiClient();
	}

	[HttpGet]
    [Route("/status/{organization}/{project}/{stack}")]
    public async Task<IResult> GetUpdateId(string organization, string project, string stack)
    {
        StatusResponse status;
        try
        {
            var result = await _api.ListStackUpdatesLatest(organization,project,stack);
            var pulumiUrl = await _api.GetStackUpdatesLatestUrl(organization, project, stack);
            if (result.Info == null)
            {
                status = new StatusResponse
                {
                    UpdateID = result.UpdateID,
                    StartTime = new DateTime(),
                    EndTime= new DateTime(),
                    Result = null,
                    CreateCount = 0,
                    DeleteCount = 0,
                    SameCount = 0,
                    UpdateCount= 0,
                    UpdateUrl = pulumiUrl
                };
            }
            else
            {
                status = new StatusResponse
                {
                    UpdateID = result.UpdateID,
                    StartTime = result.Info.StartTimeDt,
                    EndTime= result.Info.EndTimeDt,
                    Result = result.Info.Result,
                    CreateCount = result.Info.ResourceChanges?.Create == null ? 0 : result.Info.ResourceChanges.Create ,
                    DeleteCount = result.Info.ResourceChanges?.Delete == null ? 0 : result.Info.ResourceChanges.Delete,
                    SameCount = result.Info.ResourceChanges?.Same == null ? 0 : result.Info.ResourceChanges.Same,
                    UpdateCount= result.Info.ResourceChanges?.Update == null ? 0 : result.Info.ResourceChanges.Update,
                    UpdateUrl = pulumiUrl

                };
            }
            return Results.Ok(status);
        }
        catch (Exception ex)
        {
            return Results.Accepted("OK",ex.Message);
        }
    }
}


public class StatusResponse
{
    [JsonPropertyName("UpdateID")]
    public string? UpdateID { get; set;}

    [JsonPropertyName("StartTime")]
    public DateTime StartTime {get;set;}

    [JsonPropertyName("EndTime")]
    public DateTime EndTime {get;set;}

    [JsonPropertyName("Result")]
    public string? Result {get;set;}

    [JsonPropertyName("DeleteCount")]
    public int? DeleteCount {get; set;} = 0;

    [JsonPropertyName("CreateCount")]
    public int? CreateCount {get; set;} = 0;

    [JsonPropertyName("SameCount")]
    public int? SameCount {get; set;} = 0;

    [JsonPropertyName("UpdateCount")]
    public int? UpdateCount {get; set;} = 0;

    [JsonPropertyName("UpdateUrl")]
    public string? UpdateUrl {get;set;}
}

