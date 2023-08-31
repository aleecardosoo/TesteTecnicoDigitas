using System.Globalization;
using TesteTecnicoDigitas.Domain.Models;

namespace TesteTecnicoDigitas.Infrastructure.Services
{
    public class ComumWebSocketServices
    {
        public async Task ProcessGroupedMessages(List<OrderBook> orderBooksCollection, string instrument)
        {
            decimal maxPriceAsk = await MaxPriceAsks(orderBooksCollection);
            var minPriceAsk = await MinPriceAsks(orderBooksCollection);
            var averagePriceMoment = await AveragePriceMoment(orderBooksCollection);
            var averageAccumulatedPrice = await AverageAccumulatedPrice(orderBooksCollection);
            var averageAccumulatedQuantity = await AverageAccumulatedQuantity(orderBooksCollection);

            Console.WriteLine($"Valores para {instrument} {DateTime.Now}: \n\t{maxPriceAsk} maior preço pedido \n\t{minPriceAsk} menor preço pedido \n\t{averagePriceMoment} Média de preço nesse momento \n\t{averageAccumulatedPrice} Média de preço acumulada nos últimos 5 segundos \n\t{averageAccumulatedQuantity} Média da quantidade acumulada nos últimos 5 segundos");
        }

        public async Task<decimal> MaxPriceAsks(List<OrderBook> orderBooksCollection)
        {
            var cultureInfo = new CultureInfo("en-US");
            decimal maxPriceAsk = 0;
            foreach (OrderBook orderBook in orderBooksCollection)
            {
                foreach (List<string> ask in orderBook.Data.Asks)
                {
                    var askValue = decimal.Parse(ask[0], cultureInfo);
                    if (askValue > maxPriceAsk)
                    {
                        maxPriceAsk = askValue;
                    }
                }
            }
            return maxPriceAsk;
        }

        public async Task<decimal> MinPriceAsks(List<OrderBook> orderBooksCollection)
        {
            var cultureInfo = new CultureInfo("en-US");
            decimal minPriceAsk = decimal.MaxValue;
            foreach (OrderBook orderBook in orderBooksCollection)
            {
                foreach (List<string> ask in orderBook.Data.Asks)
                {
                    var askValue = decimal.Parse(ask[0], cultureInfo);
                    if (askValue < minPriceAsk)
                    {
                        minPriceAsk = askValue;
                    }
                }
            }
            return minPriceAsk;
        }

        public async Task<decimal> AveragePriceMoment(List<OrderBook> orderBooksCollection)
        {
            var cultureInfo = new CultureInfo("en-US");
            decimal accumulatedPrice = 0;
            decimal totalOrders = 0;
            var lastOrder = orderBooksCollection.Last();
            foreach (List<string> ask in lastOrder.Data.Asks)
            {
                var askValue = decimal.Parse(ask[0], cultureInfo);
                accumulatedPrice += askValue;
                totalOrders++;
            }
            return accumulatedPrice / totalOrders;
        }

        private async Task<decimal> AverageAccumulatedPrice(List<OrderBook> orderBooksCollection)
        {
            var cultureInfo = new CultureInfo("en-US");
            decimal accumulatedPrice = 0;
            decimal totalOrders = 0;
            foreach (OrderBook orderBook in orderBooksCollection)
            {
                foreach (List<string> ask in orderBook.Data.Asks)
                {
                    var askValue = decimal.Parse(ask[0], cultureInfo);
                    accumulatedPrice += askValue;
                    totalOrders++;
                }
            }
            return accumulatedPrice / totalOrders;
        }

        private async Task<decimal> AverageAccumulatedQuantity(List<OrderBook> orderBooksCollection)
        {
            var cultureInfo = new CultureInfo("en-US");
            decimal accumulatedQuantity = 0;
            decimal totalOrders = 0;
            foreach (OrderBook orderBook in orderBooksCollection)
            {
                foreach (List<string> ask in orderBook.Data.Asks)
                {
                    var askQuantity = decimal.Parse(ask[1], cultureInfo);
                    accumulatedQuantity += askQuantity;
                    totalOrders++;
                }
            }
            return accumulatedQuantity / totalOrders;
        }

    }
}
