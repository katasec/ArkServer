using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ArkServer;

public struct ArkConfig
{
    [YamlMember(Alias = "cloudid")]
    public string CloudId { get; set; }

    [YamlMember(Alias = "azureconfig")]
    public AzureConfig AzureConfig { get; set; }

    [YamlMember(Alias = "awsconfig")]
    public AwsConfig AwsConfig { get; set; }

    [YamlMember(Alias = "logfile")]
    public string LogFile { get; set; }

    [YamlMember(Alias = "pulumidefultorg")]
    public string PulumiDefaultOrg { get; set; }

    public static readonly string UserHomeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    public static readonly string ArkHome = Path.Join(UserHomeDir, ".ark");
    public static readonly string ConfigFile = Path.Join(ArkHome , "config");


    public static ArkConfig Read()
    {

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var configTxt = File.ReadAllText(ConfigFile);

        var config = deserializer.Deserialize<ArkConfig>(configTxt);

        return config;
    }

}

public struct AwsConfig
{
    [YamlMember(Alias = "storageconfig")]
    public StorageConfig StorageConfig { get; set; }

    [YamlMember(Alias = "mqconfig")]
    public MqConfig MqConfig { get; set; }
}

public struct AzureConfig
{
    [YamlMember(Alias = "resourcegroupname")]
    public string ResourceGroupName { get; set; }

    [YamlMember(Alias = "storageconfig")]
    public StorageConfig StorageConfig { get; set; }

    [YamlMember(Alias = "mqconfig")]
    public MqConfig MqConfig{ get; set; }

}

public struct StorageConfig
{
    [YamlMember(Alias = "logstorageaccountname")]
    public string LogStorageAccountName { get; set; }

    [YamlMember(Alias = "logstorageendpoint")]
    public string LogStorageEndpoint { get; set; }

    [YamlMember(Alias = "logscontainer")]
    public string LogsContainer { get; set; }

    [YamlMember(Alias = "logstoragekey")]
    public string LogStorageKey { get; set; }

    [YamlMember(Alias = "pulumistatecontainer")]
    public string PulumiStateContainer { get; set; }

}
public struct MqConfig
{
    [YamlMember(Alias = "mqconnectionstring")]
    public string MqConnectionString { get; set; }

    [YamlMember(Alias = "mqname")]
    public string MqName { get; set; }
}