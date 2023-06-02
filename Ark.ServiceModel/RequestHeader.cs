using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ark.ServiceModel;

public class RequestHeader
{
    required public string  Action { get; set; }
    required public string Resource { get; set; }
}
