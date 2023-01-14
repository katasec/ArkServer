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

	public AzureCloudspace GenAzureCloudspace(int hubOctet1=10, int hubOctet2=16)
	{
		//var generator = new CidrGenerator(octet1, octet2);

		
  //      var cs = new AzureCloudspace
  //      {
		//	Hub = generator.Hub,
  //          Spokes = generator.Spokes(_request.Environments)
  //      };

		//return cs;

		return null;
	}
}
