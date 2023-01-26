using ArkServer.Entities.Azure;
using System.Text.Json;

namespace ArkServer.Repositories;

public interface ICloudspaceRepo
{
    Ark Get();
    bool Save(Ark ark);
}

public class CloudspaceJsonRepo : ICloudspaceRepo
{
    private string DbDir { get; }
    private string DbFile { get; }
    private ILogger _logger;

    public CloudspaceJsonRepo(ILogger<CloudspaceJsonRepo> logger)
    {
        _logger = logger;

        // Define DB path & File
        DbDir = Path.Join(ArkConfig.ArkHome, "db");
        if (!Directory.Exists(DbDir))
        {
            Directory.CreateDirectory(DbDir);
        }
        DbFile = Path.Join(DbDir, "ark.json");

    }
    public Ark Get()
    {
        Ark emptyArk = new();
        if (!File.Exists(DbFile))
        {
            _logger.Log(LogLevel.Information, $"{DbFile} doesn't exist!");
            return emptyArk;
        }
        else
        {
            _logger.Log(LogLevel.Information, "Reading DB");
        }


        try
        {
            var jsonString = File.ReadAllText(DbFile);
            var arkFromFile = JsonSerializer.Deserialize<Ark>(jsonString);
            return arkFromFile != null ? arkFromFile : emptyArk;
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error,ex.Message);
            return emptyArk;
        }
    }

    public bool Save(Ark ark)
    {

        // Generate JSON from Ark object
        var jsonString = JsonSerializer.Serialize(ark, new JsonSerializerOptions() { WriteIndented = true });

        // Save to disk
        try
        {
            _logger.Log(LogLevel.Information, "Saving data to " + DbFile);
            File.WriteAllText(DbFile, jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }


        return true;
    }
}
