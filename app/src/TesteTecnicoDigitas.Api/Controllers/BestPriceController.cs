using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using System.Security.Cryptography.X509Certificates;
using TesteTecnicoDigitas.Domain.Commands;
using TesteTecnicoDigitas.Domain.DataServices;
using TesteTecnicoDigitas.Domain.Models;
using TesteTecnicoDigitas.Domain.Repository;
using TesteTecnicoDigitas.Domain.Services;
using TesteTecnicoDigitas.Infrastructure.Repository;

namespace TesteTecnicoDigitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestPriceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly IWebSocketService _socketService;
        private readonly IBestPriceRepository _bestPriceRepository;
        public BestPriceController(IMediator mediator, IConfiguration configuration, IWebSocketService socketService, IBestPriceRepository bestPriceRepository)
        {
            _mediator = mediator;
            _configuration = configuration;
            _socketService = socketService;
            _bestPriceRepository = bestPriceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BestPriceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
