namespace WebKit.Core.Errors;

/// <summary>
/// A stable, machine-readable application error.
/// </summary>
public sealed record WebKitError(string Code, string Message)
{
    public static WebKitError None { get; } = new(string.Empty, string.Empty);

    public static WebKitError Validation(string message) => new("validation", message);

    public static WebKitError NotFound(string message = "The requested resource was not found.") => new("not_found", message);

    public static WebKitError Forbidden(string message = "You are not allowed to perform this action.") => new("forbidden", message);

    public static WebKitError Conflict(string message) => new("conflict", message);
}
