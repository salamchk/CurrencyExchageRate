﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Models
{
    public class CurrencyExchangeRate
    {
        [JsonProperty(PropertyName = "r030")]
        public int r030 { get; set; }

        [JsonProperty(PropertyName = "txt")]
        public string txt { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public float rate { get; set; }

        [JsonProperty(PropertyName ="cc")]
        public string cc { get; set; }

        [JsonProperty(PropertyName ="exchangedate")]
        public string exchangedate { get; set; }

    }
}
