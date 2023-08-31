using TesteTecnicoDigitas.Domain.DataServices;
using TesteTecnicoDigitas.Domain.Repository;
using TesteTecnicoDigitas.Domain.Services;
using TesteTecnicoDigitas.Infrastructure.DataServices;
using TesteTecnicoDigitas.Infrastructure.Services;

namespace TesteTecnicoDigitas.Api
{
    public class StartupBtc
    {
        private readonly IConfiguration _configuration;
        private readonly IWebSocketService _socketService;
        private readonly IWebSocketDataService _socketDataService;
        private readonly IOrderBookRepository _socketRepository;

        public StartupBtc(IConfiguration configuration, IWebSocketService socketService, IWebSocketDataService socketDataService, IOrderBookRepository socketRepository)
        {
            _configuration = configuration;
            _socketService = socketService;
            _socketRepository = socketRepository;
            _socketDataService = new WebSocketDataService(_socketRepository);

            InitializeBtcServices();
        }

        private async Task InitializeBtcServices()
        {
            var btcService = new WebSocketService(_configuration, _socketDataService);
            await btcService.StartGetData(
                _configuration["web_socket_connection_uri"],
                _configuration["subscribe_event"],
                _configuration[$"channels:live_order_book_btcusd"],
                "BTC/USD"
            );
        }
    }
}
