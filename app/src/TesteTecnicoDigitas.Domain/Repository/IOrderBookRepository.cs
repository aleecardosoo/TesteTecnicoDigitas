using TesteTecnicoDigitas.Domain.Models;

namespace TesteTecnicoDigitas.Domain.Repository
{
    public interface IOrderBookRepository
    {
        Task SaveOrderBookAsync(OrderBook orderBook, string instrument);
        Task<OrderBook> GetLastOrderBook(string instrument);
    }
}
