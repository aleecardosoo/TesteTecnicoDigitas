using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using TesteTecnicoDigitas.Domain.Models;
using TesteTecnicoDigitas.Domain.DataServices;
using TesteTecnicoDigitas.Domain.Repository;
using TesteTecnicoDigitas.Infrastructure.Services;
using Timer = System.Threading.Timer;

namespace TesteTecnicoDigitas.Infrastructure.DataServices
{
    public class WebSocketDataService : IWebSocketDataService
    {
        private List<OrderBook> btcusdCollection = new List<OrderBook>();
        private List<OrderBook> ethusdCollection = new List<OrderBook>();
        private readonly object lockObject = new object();
        private static OrderBook lastBtcusdReceived;
        private static OrderBook lastEthusdReceived;
        private string _instrument;
        private readonly IOrderBookRepository _orderBookRepository;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public WebSocketDataService(IOrderBookRepository orderBookRepository)
        {
            _orderBookRepository = orderBookRepository;
        }

        public async Task<ClientWebSocket> MakeConnection(string uri, string instrument)
        {
            _instrument = instrument;
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
            return clientWebSocket;
        }

        public async Task CloseConnection(ClientWebSocket clientWebSocket, string reason)
        {
            _cancellationTokenSource.Cancel();
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, reason, CancellationToken.None);
        }

        public async Task SendMessage(ClientWebSocket clientWebSocket, SubscriptionMessage message)
        {
            var messageString = JsonConvert.SerializeObject(message);
            byte[] buffer = Encoding.UTF8.GetBytes(messageString);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task ReceiveMessages(ClientWebSocket clientWebSocket)
        {
            byte[] buffer = new byte[1048576];

            Timer timer = new Timer(TimerElapsed, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            try
            {
                while (clientWebSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    string jsonResponse = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var orderBook = JsonConvert.DeserializeObject<OrderBook>(jsonResponse);

                    if (orderBook != null && !orderBook.Data.IsDataEmpty())
                    {

                        lock (lockObject)
                        {
                            if (string.Equals(_instrument, "BTC/USD"))
                            {
                                lastBtcusdReceived = orderBook;
                                btcusdCollection.Add(orderBook);
                                _orderBookRepository.SaveOrderBookAsync(orderBook, _instrument);
                            }
                            if (string.Equals(_instrument, "ETH/USD"))
                            {
                                lastEthusdReceived = orderBook;
                                ethusdCollection.Add(orderBook);
                                _orderBookRepository.SaveOrderBookAsync(orderBook, _instrument);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, ex.Message, CancellationToken.None);
            }
            finally
            {
                timer.Dispose();
            }
        }

        private void TimerElapsed(object state)
        {
            lock (lockObject)
            {
                if (string.Equals(_instrument, "BTC/USD"))
                {
                    var messagesToProcess = new List<OrderBook>(btcusdCollection);
                    btcusdCollection.Clear();
                    ComumWebSocketServices btcServices = new ComumWebSocketServices();
                    btcServices.ProcessGroupedMessages(messagesToProcess, _instrument);
                }
                if (string.Equals(_instrument, "ETH/USD"))
                {
                    var messagesToProcess = new List<OrderBook>(ethusdCollection);
                    ethusdCollection.Clear();
                    ComumWebSocketServices ethServices = new ComumWebSocketServices();
                    ethServices.ProcessGroupedMessages(messagesToProcess, _instrument);
                }
            }
        }
    }
}
