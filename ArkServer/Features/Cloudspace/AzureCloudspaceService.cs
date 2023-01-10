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
		Console.WriteLine(JsonSerializer.Serialize(_request, new JsonSerializerOptions
		{
			WriteIndented=true
		}));


        var cs = new AzureCloudspace
        {
            Name = _request.Name,
			Hub = new ReferenceNetwork().Hub,
            Env = new List<VNetInfo> { }
        };


        for (var i =0; i< _request.Environments.Count(); i++)
		{
			Console.WriteLine(_request.Environments[i]);
		}


		//var cs = new AzureCloudspace
		//{
		//	Name= _request.Name,
		//};
		//var azureCs = new AzureCloudspace(
		//	Name: _request.ProjectName,
		//	Envs: _request.Spokes
		//);

		//return azureCs;
	}
}
