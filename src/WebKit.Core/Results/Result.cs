using WebKit.Core.Errors;

namespace WebKit.Core.Results;

/// <summary>
/// Represents either a successful operation or one meaningful application error.
/// </summary>
public readonly record struct Result<T>
{
    private Result(T value, WebKitError error, bool isSuccess)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value { get; }

    public WebKitError Error { get; }

    public static Result<T> Success(T value) => new(value, WebKitError.None, true);

    public static Result<T> Failure(WebKitError error) => new(default!, error, false);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<WebKitError, TResult> onFailure) => IsSuccess ? onSuccess(Value) : onFailure(Error);
}
