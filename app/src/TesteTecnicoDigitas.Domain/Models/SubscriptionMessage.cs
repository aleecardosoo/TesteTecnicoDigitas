using Newtonsoft.Json;

namespace TesteTecnicoDigitas.Domain.Models
{
    public class SubscriptionMessage
    {
        public SubscriptionMessage(string @event, string channel)
        {
            Event = @event;
            Data = new DataObject(channel);
        }
        [JsonProperty("event")]
        public string Event { get; set; }
        [JsonProperty("data")]
        public DataObject Data { get; set; }
    }

    public class DataObject
    {
        public DataObject(string channel)
        {
            Channel = channel;
        }
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
