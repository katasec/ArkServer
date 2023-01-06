namespace ArkServer.Features.Cloudspace;

using ArkServer.Entities.Azure;
using JsonFlatFileDataStore;

/// <summary>
/// IAzureCsRepo is an Azure 'Cloudspace' Repository
/// </summary>
public interface IAzureCsRepo
{
    IEnumerable<AzureCloudspace> GetClouspaces();
    AzureCloudspace GetCloudspaceById(int id);
    AzureCloudspace? Create(AzureCloudspace cloudspace);
}

public class CloudspaceJsonRepository : IAzureCsRepo
{
    private string DbDir { get; }
    private string DbFile { get; }
    private DataStore Store { get; }

    private IDocumentCollection<AzureCloudspace> Collection { get; }

    public CloudspaceJsonRepository()
    {
        // Define DB path & File
        DbDir = Path.Join(ArkConfig.ArkHome, "db");
        if (!Directory.Exists(DbDir))
        {
            Directory.CreateDirectory(DbDir);
        }
        DbFile = Path.Join(DbDir, "data.json");

        // Create Json Data store
        Store = new DataStore(DbFile);

        // Get employee collection
        Collection = Store.GetCollection<AzureCloudspace>();

    }

    public IEnumerable<AzureCloudspace> GetClouspaces()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AzureCloudspace> GetCloudpace(string name)
    {
        throw new NotImplementedException();
    }

    public AzureCloudspace GetCloudspaceById(int id)
    {
        throw new NotImplementedException();
    }

    public AzureCloudspace Create(AzureCloudspace cloudspace)
    {
        if (Collection.Count == 0)
        {
            if (Collection.InsertOne(cloudspace)) return cloudspace;
            throw new ApplicationException("could not save collection");
        }
        else
        {
            var results = Collection.AsQueryable().Where(x => x.ProjectName == cloudspace.ProjectName).First();


            if (results != null)
            {
                Console.WriteLine("Item already exists");
                return cloudspace;
            }

        }

        if (Collection.InsertOne(cloudspace)) return cloudspace;
        throw new ApplicationException("could not save collection");
    }
}

