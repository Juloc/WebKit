namespace WebKit.UI;

public sealed record PageHeaderModel(string Title, string? Description = null, string? ActionText = null, string? ActionUrl = null);

public sealed record BreadcrumbItemModel(string Text, string? Url = null);

public sealed record BreadcrumbsModel(IReadOnlyList<BreadcrumbItemModel> Items);

public sealed record NavigationItemModel(string Text, string Url, bool IsCurrent = false);

public sealed record NavigationGroupModel(string? Label, IReadOnlyList<NavigationItemModel> Items);

public sealed record NavigationModel(IReadOnlyList<NavigationGroupModel> Groups);

public sealed record LoadingStateModel(string Label = "Loading");

public enum AlertTone
{
    Info,
    Success,
    Warning,
    Error
}

public sealed record AlertModel(string Text, AlertTone Tone = AlertTone.Info);

public sealed record StatCardModel(string Label, string Value, string? Description = null);

public sealed record FactsListModel(IReadOnlyList<(string Label, string Value)> Items);

public sealed record SettingsItemModel(string Label, string Description, string Value);

public sealed record SettingsSectionModel(string Id, string Heading, string Explanation, IReadOnlyList<SettingsItemModel> Items);

public sealed record EmptyStateModel(string Title, string Description, string? ActionText = null, string? ActionUrl = null);

public sealed record ErrorStateModel(string Title = "Something went wrong", string Description = "Try again or contact support if the problem continues.");

public sealed record ForbiddenStateModel(string Title = "Access denied", string Description = "You do not have permission to view this page.");

public enum StatusTone
{
    Neutral,
    Info,
    Success,
    Warning,
    Danger
}

public sealed record StatusBadgeModel(string Text, StatusTone Tone = StatusTone.Neutral);

public sealed record PaginationModel(int Page, int PageCount, string BaseUrl, IReadOnlyDictionary<string, string?>? Query = null)
{
    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < PageCount;

    public string UrlFor(int page)
    {
        List<string> parameters = [$"page={page}"];
        if (Query is not null)
        {
            parameters.AddRange(Query
                .Where(parameter => !string.Equals(parameter.Key, "page", StringComparison.OrdinalIgnoreCase) && parameter.Value is not null)
                .Select(parameter => $"{Uri.EscapeDataString(parameter.Key)}={Uri.EscapeDataString(parameter.Value!)}"));
        }

        return $"{BaseUrl}{(BaseUrl.Contains('?', StringComparison.Ordinal) ? '&' : '?')}{string.Join('&', parameters)}";
    }
}

public sealed record ConfirmDialogModel(string DialogId, string Title, string Description, string ActionUrl, string ConfirmText = "Confirm", string CancelText = "Cancel")
{
    public string TitleId => $"{DialogId}-title";
}

public enum ToastKind
{
    Information,
    Success,
    Warning,
    Error
}

public sealed record ToastMessageModel(string Text, ToastKind Kind, bool IsAssertive = false);

public sealed record ToastModel(IReadOnlyList<ToastMessageModel> Messages);
