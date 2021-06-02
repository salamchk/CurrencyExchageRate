using Newtonsoft.Json;

namespace CurrencyExchageRate.Models
{
    public class ExchangeRate
    {
        [JsonProperty(PropertyName = "r030")]
        public int Indetifier { get; set; }

        [JsonProperty(PropertyName = "txt")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public float Rate { get; set; }

        [JsonProperty(PropertyName ="cc")]
        public string ShortName { get; set; }

        [JsonProperty(PropertyName ="exchangedate")]
        public string ExchangeDate { get; set; }

    }
}
