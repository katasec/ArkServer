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

	public AzureCloudspace GenAzureCloudspace(int octet1=10, int octet2=16)
	{
		var generator = new CIDRGenerator(octet1, octet2);

        var cs = new AzureCloudspace
        {
            Name = _request.Name,
			Hub = generator.Hub,
            Env = generator.Spokes(_request.Environments)
        };

		return cs;
	}
}
