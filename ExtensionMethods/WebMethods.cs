namespace ArkServer.ExtensionMethods;

public static class WebMethods
{
    /// <summary>
    /// Get() registers a provided delegate func as a handler for GET requests at the given pattern.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern">URI pattern used for this HTTP request</param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static WebApplication Get(this WebApplication app, Delegate handler, string pattern, string description="", string displayName="", string summary="" )
    {
        app.MapGet(pattern, handler)
            .WithName(pattern)
            .WithDescription(description)
            .WithDisplayName(displayName)
            .WithSummary(summary)
            .WithOpenApi();
        return app;
    }

    /// <summary>
    /// Post() registers a provided delegate func as a handler for POST requests at the given pattern.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern">URI pattern used for this HTTP request</param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static WebApplication Post(this WebApplication app, string pattern, Delegate handler)
    {
        app.MapPost(pattern, handler).WithName(pattern).WithOpenApi();
        return app;
    }

    /// <summary>
    /// Post() registers a provided delegate func as a handler for DELETE requests at the given pattern.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="pattern">URI pattern used for this HTTP request</param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static WebApplication Delete(this WebApplication app, string pattern, Delegate handler)
    {
        app.MapDelete(pattern, handler).WithName(pattern).WithOpenApi();
        return app;
    }
}

