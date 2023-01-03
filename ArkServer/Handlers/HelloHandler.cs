namespace ArkServer.Handlers;

public static partial class HandlerFunc
{
    public static Func<string,string> HelloHandler = (name) => $"Hello {name}";
}