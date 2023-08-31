using System.Net.WebSockets;
using TesteTecnicoDigitas.Domain.Models;

namespace TesteTecnicoDigitas.Domain.DataServices
{
    public interface IWebSocketDataService
    {
        Task<ClientWebSocket> MakeConnection(string uri, string instrument);
        Task CloseConnection(ClientWebSocket clientWebSocket, string reason);
        Task SendMessage(ClientWebSocket clientWebSocket, SubscriptionMessage message);
        Task ReceiveMessages(ClientWebSocket clientWebSocket);
    }
}
