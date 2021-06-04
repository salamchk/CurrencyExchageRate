using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace CurrencyRateLibrary.WebClientBank
{
    public class WebApiDataProvider : IApiProvider
    {
        private readonly string _mainPartOfUri;
        private const string _jsonPart = "&json";
        private const string _dateFormat = "yyyyMMdd";
        public WebApiDataProvider(string partOfUr)
        {
            _mainPartOfUri = partOfUr;
        }

        public List<ExchangeRate> GetCurrencyExchangeRate(DateTime time)
        {
            var datePartOfUr = time.ToString(_dateFormat);
            var uri = _mainPartOfUri + datePartOfUr + _jsonPart;

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
