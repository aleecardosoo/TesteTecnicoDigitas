using MediatR;
using Newtonsoft.Json;
using TesteTecnicoDigitas.Domain.Models;

namespace TesteTecnicoDigitas.Domain.Commands
{
    public class BestPriceCommand : IRequest<BestPrice>
    {
        [JsonProperty("operation")]
        public string Operation { get; set; }
        [JsonProperty("instrument")]
        public string Instrument { get; set; }
        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }
    }
}
