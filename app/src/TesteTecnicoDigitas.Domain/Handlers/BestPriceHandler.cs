using MediatR;
using System.Globalization;
using TesteTecnicoDigitas.Domain.Commands;
using TesteTecnicoDigitas.Domain.Models;
using TesteTecnicoDigitas.Domain.Repository;

namespace TesteTecnicoDigitas.Domain.Handlers
{
    public class BestPriceHandler : IRequestHandler<BestPriceCommand, BestPrice>
    {
        private readonly IOrderBookRepository _orderBookRepository;
        private readonly IBestPriceRepository _bestPriceRepository;
        private List<List<string>> usedCollection = new List<List<string>>();
        public BestPriceHandler(IOrderBookRepository orderBookRepository, IBestPriceRepository bestPriceRepository)
        {
            _orderBookRepository = orderBookRepository;
            _bestPriceRepository = bestPriceRepository;
        }

        public async Task<BestPrice> Handle(BestPriceCommand request, CancellationToken cancellationToken)
        {
            var orderBook = await _orderBookRepository.GetLastOrderBook(request.Instrument);

            List<List<string>> dataList = request.Operation.Equals("buy") ? ArrangeListAscendingOrder(orderBook.Data.Asks) : ArrangeListDescendingOrder(orderBook.Data.Bids); 

            var totalPrice = CalculatePrice(dataList, request.Quantity);

            var result = new BestPrice
            {
                Id = Guid.NewGuid(),
                Operation = request.Operation,
                Quantity = request.Quantity,
                Result = totalPrice,
                Collection = request.Operation.Equals("buy") ? ArrangeListAscendingOrder(usedCollection) : ArrangeListAscendingOrder(usedCollection)
            };

            await _bestPriceRepository.SaveBestPriceAsync(result, request.Instrument);

            return result;
        }

        private List<List<string>> ArrangeListAscendingOrder(List<List<string>> list)
        {
            var cultureInfo = new CultureInfo("en-US");
            var arrangedList = new List<List<string>>();
            decimal control = decimal.MinValue;

            foreach (List<string> item in list)
            {
                var price = decimal.Parse(item[0], cultureInfo);

                if (price >= control)
                {
                    control = price;
                    arrangedList.Insert(0, item);
                }
                else
                {
                    for (int i = 0; i < arrangedList.Count; i++)
                    {
                        var arrangedPrice = decimal.Parse(arrangedList[i][0], cultureInfo);
                        if (price < arrangedPrice)
                        {
                            arrangedList.Insert(i, item);
                            break;
                        }
                    }
                }
            }
            return arrangedList;
        }

        private List<List<string>> ArrangeListDescendingOrder(List<List<string>> list)
        {
            var cultureInfo = new CultureInfo("en-US");
            var arrangedList = new List<List<string>>();
            decimal control = decimal.MaxValue;

            foreach (List<string> item in list)
            {
                var price = decimal.Parse(item[0], cultureInfo);

                if (price <= control)
                {
                    control = price;
                    arrangedList.Insert(0, item);
                }
                else
                {
                    for (int i = 0; i < arrangedList.Count; i++)
                    {
                        var arrangedPrice = decimal.Parse(arrangedList[i][0], cultureInfo);
                        if (price > arrangedPrice)
                        {
                            arrangedList.Insert(i, item);
                            break;
                        }
                    }
                }
            }
            return arrangedList;
        }

        private decimal CalculatePrice(List<List<string>> list, decimal requestedQuantity)
        {
            var cultureInfo = new CultureInfo("en-US");
            decimal accumulatedQuantity = 0;
            decimal totalPrice = 0;
            decimal result = 0;

            foreach (List<string> item in list)
            {
                var price = decimal.Parse(item[0], cultureInfo);
                var quantity = decimal.Parse(item[1], cultureInfo);

                if (accumulatedQuantity + quantity <= requestedQuantity)
                {
                    accumulatedQuantity += quantity;
                    totalPrice = price * quantity;
                    result += totalPrice;
                    usedCollection.Add(item);
                }
                else
                {
                    decimal remainingQuantity = requestedQuantity - accumulatedQuantity;
                    totalPrice = (price * remainingQuantity);
                    result += totalPrice;
                    accumulatedQuantity = requestedQuantity;
                    usedCollection.Add(item);
                    break;
                }
            }
            return result;
        }
    }
}
