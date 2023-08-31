using TesteTecnicoDigitas.Domain.Models;

namespace TesteTecnicoDigitas.Domain.Repository
{
    public interface IBestPriceRepository
    {
        Task SaveBestPriceAsync(BestPrice bestPrice, string instrument);
    }
}
