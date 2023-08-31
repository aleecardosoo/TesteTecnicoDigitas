using System.Net.WebSockets;

namespace TesteTecnicoDigitas.Domain.Services
{
    public interface IWebSocketService
    {
        Task StartGetData(string uri, string @event, string channel, string instrument);
        Task CloseConnection(ClientWebSocket client, string reason);
    }
}
