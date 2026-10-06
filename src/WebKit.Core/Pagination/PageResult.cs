namespace WebKit.Core.Pagination;

/// <summary>
/// A page of items with enough metadata for a reusable pagination component.
/// </summary>
public sealed record PageResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
{
    public int PageCount => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < PageCount;
}
