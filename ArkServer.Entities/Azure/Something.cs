using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArkServer.Entities.Azure;

public class Something
{
    [JsonPropertyName("Outputs")]
    public Outputs1 Outputs { get; set; }
}

public class Outputs1
{
    [JsonPropertyName("addressSpace")]
    public AddressSpace1? AddressSpace { get; set; }

}


public class AddressSpace1
{
    [JsonPropertyName("addressPrefixes")]
    public List<string>? AddressPrefixes { get; set; }
}