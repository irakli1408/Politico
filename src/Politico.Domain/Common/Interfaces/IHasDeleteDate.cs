namespace Politico.Domain.Common.Interfaces
{
    public interface IHasDeleteDate
    {
        DateTime? DeleteDate { get; set; }
    }
}
