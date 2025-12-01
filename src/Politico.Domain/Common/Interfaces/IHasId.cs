namespace Politico.Domain.Common.Interfaces
{
    public interface IHasId<TKey>
    {
        TKey Id { get; }
    }
}
