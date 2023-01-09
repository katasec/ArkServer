using ArkServer.Entities.Azure;
using JsonFlatFileDataStore;
using System.Collections.Generic;
using System.Collections;
using System.Text.Json;
using YamlDotNet.Core;

namespace ArkServer.Features.Cloudspace;

public interface IArkRepo
{
    Ark Get();
    bool Save(Ark ark);
}

public class ArkJsonRepo : IArkRepo
{
    private string DbDir { get; }
    private string DbFile { get; }
    private ILogger _logger;

    public ArkJsonRepo(ILogger<ArkJsonRepo> logger)
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
        _logger.Log(LogLevel.Information,"Reading DB");

        Ark emptyArk = new();

        if (!File.Exists(DbFile))
        {
            _logger.Log(LogLevel.Information, $"{DbFile} doesn't exist!");
            return emptyArk;
        }

        
        try {
            var jsonString = File.ReadAllText(DbFile);
            var arkFromFile = JsonSerializer.Deserialize<Ark>(jsonString);
            return arkFromFile != null ? arkFromFile : emptyArk ;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return emptyArk;
        }
    }

    public bool Save(Ark ark)
    {

        var jsonString = JsonSerializer.Serialize(ark,new JsonSerializerOptions(){
         WriteIndented = true
        });

        try
        {
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
