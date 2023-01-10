namespace ArkServer.Entities.Azure;

public record SubnetInfo(
    string Name, string AddressPrefix, string Description, 
    KeyValuePair<string,string> Tags = new KeyValuePair<string,string>()
);
