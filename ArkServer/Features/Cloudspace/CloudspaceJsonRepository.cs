namespace ArkServer.Features.Cloudspace;

using ArkServer.Entities.Azure;

public interface ICloudspaceRepository
{
    IEnumerable<AzureCloudspace> GetClouspaces();
    AzureCloudspace GetCloudspaceByID(int id);

    AzureCloudspace Create(AzureCloudspace cloudspace);
}

public class CloudspaceJsonRepository : ICloudspaceRepository
{
    public CloudspaceJsonRepository()
    {

    }

    public IEnumerable<AzureCloudspace> GetClouspaces()
    {
        throw new NotImplementedException();
    }

    public AzureCloudspace GetCloudspaceByID(int id)
    {
        throw new NotImplementedException();
    }

    public AzureCloudspace Create(AzureCloudspace cloudspace)
    {
        throw new NotImplementedException();
    }
}

