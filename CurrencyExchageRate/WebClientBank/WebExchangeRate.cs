using CurrencyExchageRate.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Models
{
    public class WebExchangeRate : IDataBankApiProvider
    {
        private static WebExchangeRate _webExchangeRate;
        private const string partOfUR = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date=";
        private WebExchangeRate()
        {
        }

        public static WebExchangeRate GetWebExchangeRate()
        {
            return _webExchangeRate ?? new WebExchangeRate();
        }

        public List<ExchangeRate> GetRates(DateTime time)
        {
            var uri = partOfUR + time.ToString("yyyyMMdd") + @"&json"; 
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
            var listOfCurrencies = JsonConvert.DeserializeObject<List<ExchangeRate>>(webResponce);
            return listOfCurrencies;
        }
    }
}
