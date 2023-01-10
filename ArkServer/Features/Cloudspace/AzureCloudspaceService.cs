using ArkServer.Entities.Azure;
using System.Text.Json;

namespace ArkServer.Features.Cloudspace;


public class AzureCloudspaceService
{
	private readonly AzureCloudspaceRequest _request;
	public AzureCloudspaceService(AzureCloudspaceRequest request)
	{
		_request= request;
	}

	public void GenAzureCloudspace()
	{
		var options = new JsonSerializerOptions { WriteIndented=true};

		Console.WriteLine(JsonSerializer.Serialize(_request, options));

		var referenceNetwork = new ReferenceNetwork();
        var cs = new AzureCloudspace
        {
            Name = _request.Name,
			Hub = referenceNetwork.Hub,
            Env = referenceNetwork.Spokes(_request.Environments)
        };


		Console.WriteLine(JsonSerializer.Serialize(cs, options));
	}
}
