using WebKit.Core.Errors;

namespace WebKit.Core.Results;

/// <summary>
/// Represents either a successful operation or one meaningful application error.
/// </summary>
public sealed class Result<T>
{
    private Result(T value, WebKitError? error, bool isSuccess)
    {
        value = isSuccess ? value : default!;
        this.value = value;
        this.error = error;
        IsSuccess = isSuccess;
    }

    private readonly T value;
    private readonly WebKitError? error;

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value => IsSuccess ? value : throw new InvalidOperationException("A failed result has no value.");

    public WebKitError Error => IsSuccess ? WebKitError.None : error!;

    public static Result<T> Success(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new(value, null, true);
    }

    public static Result<T> Failure(WebKitError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (string.IsNullOrWhiteSpace(error.Code))
        {
            throw new ArgumentException("A failure must contain a real error code.", nameof(error));
        }

        return new(default!, error, false);
    }

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<WebKitError, TResult> onFailure) => IsSuccess ? onSuccess(Value) : onFailure(Error);
}
