//// Create a new instance of the Ark API sever
//var server = new ArkServer.Server(args);

//// Start it
//server.Start();

//using Katasec.PulumiApi;

using PulumiApi.Models;

var client = new PulumiApi.ApiClient();

async Task<Tuple<string?, string?, string?>?> GetStackInfo()
{
    var result = await client.ListStacks();
    if (result.Stacks != null)
    {
        var myStack = result.Stacks[0];

        var orgName = myStack.OrgName;
        var projectName = myStack.ProjectName;
        var stackName = myStack.StackName;

        return Tuple.Create(orgName, projectName, stackName);
    }
    return null;
}


var (orgName, projectName, stackName) = await GetStackInfo();
projectName = "azurecloudspace";

var result = await client.GetStackState(orgName, projectName, stackName);

if (result.Deployment != null)
{
    var resource = result.GetResourceByName(
                    type: "azure-native:network:VirtualNetwork",
                    pulumiName: "vnet-hub"
    );
    Console.WriteLine(resource.ToJson());

}
