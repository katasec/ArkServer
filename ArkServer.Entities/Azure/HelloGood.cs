using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArkServer.Entities.Azure
{
    public class HelloSuccess: BaseEntity
    {
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
        }
    }

    public class HelloFail: BaseEntity
    {
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions{WriteIndented = true});
        }
    }
}
