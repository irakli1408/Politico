namespace Politico.Application.Common.Paging;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int Skip { get; }
    public int Take { get; }
    public int Total { get; }

    public PagedResult(
        IReadOnlyList<T> items,
        int skip,
        int take,
        int total)
    {
        Items = items;
        Skip = skip;
        Take = take;
        Total = total;
    }

    public static PagedResult<T> Empty(int skip, int take)
        => new(Array.Empty<T>(), skip, take, 0);
}
