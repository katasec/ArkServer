namespace ArkServer.Entities.Azure;

public record VNetInfo(
    string Name, 
    string AddressPrefix, 
    IEnumerable<SubnetInfo> SubnetsInfo
);
