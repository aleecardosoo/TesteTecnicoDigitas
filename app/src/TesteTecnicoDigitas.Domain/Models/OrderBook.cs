using Newtonsoft.Json;

namespace TesteTecnicoDigitas.Domain.Models
{
    public class OrderBook
    {
        [JsonProperty("data")] public OrderBookData Data { get; set; }
        [JsonProperty("channel")] public string Channel { get; set; }
        [JsonProperty("event")] public string @event { get; set; }
    }
    public class OrderBookData
    {
        [JsonProperty("timestamp")] public string Timestamp { get; set; }
        [JsonProperty("microtimestamp")] public string Microtimestamp { get; set; }
        [JsonProperty("bids")] public List<List<string>> Bids { get; set; }
        [JsonProperty("asks")] public List<List<string>> Asks { get; set; }

        public bool IsDataEmpty()
        {
            if (Asks == null && Bids == null && string.IsNullOrEmpty(Timestamp) && string.IsNullOrEmpty(Microtimestamp))
                return true;
            else
                return false;
        }
    }
}
