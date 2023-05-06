using Ark.Base;
using Katasec.PulumiRunner;


//var x = new RemoteProgramArgs("aa", "aa", "aa");
//x.PulumiUp();

var c = Config.Read();

Console.WriteLine(c.AzureConfig.MqConfig.MqConnectionString);