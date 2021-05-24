using CurrencyExchageRate.Interfaces;
using Newtonsoft.Json;
using NHibernate;
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
    public class WebApiData : IDataProvider
    {
        private const string partOfUR = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date=";

        public ISession Session => throw new NotImplementedException();

        public WebApiData()
        {
        }

        public List<ExchangeRate> GetCurrencyExchangeRate(DateTime time)
        {
            var uri = partOfUR + time.ToString("yyyyMMdd") + @"&json";
            var request = WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            string webResponce;
            using (WebResponse response = request.GetResponse())
            {
                using Stream data = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(data);
                webResponce = streamReader.ReadToEnd();
            }
            var listOfCurrencies = JsonConvert.DeserializeObject<List<ExchangeRate>>(webResponce);
            return listOfCurrencies;
        }

        public List<ExchangeRate> GetCurrencyExchangeRate()
        {
            return GetCurrencyExchangeRate(DateTime.Today);
        }
    }
}
