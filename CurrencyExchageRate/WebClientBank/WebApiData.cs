using CurrencyExchageRate.Interfaces;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CurrencyExchageRate.Models
{
    public class WebApiData : IApiProvider
    {
        private readonly string _mainPartOfUri;
        private const string jsonPart = "&json";
        private const string dateFormat = "yyyyMMdd";
        public WebApiData(string partOfUr)
        {
            _mainPartOfUri = partOfUr;
        }

        public List<ExchangeRate> GetCurrencyExchangeRate(DateTime time)
        {
            var datePartOfUr = time.ToString(dateFormat);
            var uri = _mainPartOfUri + datePartOfUr + jsonPart;

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
