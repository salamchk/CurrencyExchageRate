using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Models
{
    public static class WebExchangeRate
    {
        public static List<CurrencyExchangeRate> GetRates(string uri)
        {
            var request = WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            string webResponce;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream data = response.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(data);
                    webResponce = streamReader.ReadToEnd();
                }
            }
            var listOfCurrencies = JsonConvert.DeserializeObject<List<CurrencyExchangeRate>>(webResponce);
            return listOfCurrencies;
        }
    }
}
