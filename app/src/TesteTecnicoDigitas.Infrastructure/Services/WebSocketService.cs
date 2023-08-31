using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using TesteTecnicoDigitas.Domain.DataServices;
using TesteTecnicoDigitas.Domain.Models;
using TesteTecnicoDigitas.Domain.Services;

namespace TesteTecnicoDigitas.Infrastructure.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebSocketDataService _dataService;

        public WebSocketService(IConfiguration configuration, IWebSocketDataService dataService)
        {
            _configuration = configuration;
            _dataService = dataService;
        }

        public async Task StartGetData(string uri, string @event, string channel, string instrument)
        {
            try
            {
                var client = await _dataService.MakeConnection(uri, instrument);

                var subscriptionMessage = new SubscriptionMessage(@event, channel);
                await _dataService.SendMessage(client, subscriptionMessage);

                await Task.WhenAny(_dataService.ReceiveMessages(client), Task.Delay(TimeSpan.FromSeconds(30))); // Timeout after 30 seconds
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        public async Task CloseConnection(ClientWebSocket client, string reason)
        {
            await _dataService.CloseConnection(client, reason);
        }
    }
}
