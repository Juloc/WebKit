namespace WebKit.UI;

public sealed record PageHeaderModel(string Title, string? Description = null);

public sealed record EmptyStateModel(string Title, string Description, string? ActionText = null, string? ActionUrl = null);

public sealed record ErrorStateModel(string Title = "Something went wrong", string Description = "Try again or contact support if the problem continues.");

public sealed record ForbiddenStateModel(string Title = "Access denied", string Description = "You do not have permission to view this page.");

public sealed record StatusBadgeModel(string Text, string Tone = "neutral");

public sealed record PaginationModel(int Page, int PageCount, string BaseUrl, string? Search = null)
{
    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < PageCount;

    public string UrlFor(int page) => $"{BaseUrl}?page={page}" + (string.IsNullOrWhiteSpace(Search) ? string.Empty : $"&q={Uri.EscapeDataString(Search)}");
}

public sealed record ConfirmDialogModel(string Id, string Title, string Description, string ConfirmText = "Confirm");

public sealed record ToastModel(IReadOnlyList<string> Messages);
