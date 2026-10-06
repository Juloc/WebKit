namespace WebKit.Core.Validation;

/// <summary>
/// A compact validation result used by commands and page models.
/// </summary>
public sealed class ValidationResult
{
    private readonly List<ValidationError> errors = [];

    public IReadOnlyList<ValidationError> Errors => errors;

    public bool IsValid => errors.Count == 0;

    public void Add(string field, string message) => errors.Add(new ValidationError(field, message));

    public void AddIf(bool condition, string field, string message)
    {
        if (condition)
        {
            Add(field, message);
        }
    }
}

public sealed record ValidationError(string Field, string Message);
