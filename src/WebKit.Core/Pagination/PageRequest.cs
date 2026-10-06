namespace WebKit.Core.Pagination;

/// <summary>
/// The one pagination contract used by WebKit list pages.
/// </summary>
public sealed record PageRequest
{
    public PageRequest(int page = 1, int pageSize = 20)
    {
        Page = Math.Max(1, page);
        PageSize = Math.Clamp(pageSize, 1, 100);
    }

    public int Page { get; }

    public int PageSize { get; }

    public int Skip => (Page - 1) * PageSize;
}
