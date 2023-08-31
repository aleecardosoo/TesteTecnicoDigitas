using Newtonsoft.Json;

namespace TesteTecnicoDigitas.Domain.Models
{
    public class BestPrice
    {
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("collection")] public List<List<string>> Collection { get; set; }
        [JsonProperty("quantity")] public decimal Quantity { get; set; }
        [JsonProperty("operation")] public string Operation { get; set; }
        [JsonProperty("result")] public decimal Result { get; set; }
    }
}
