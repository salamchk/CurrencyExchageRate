using LocalDbChecker.Interfaces;
using LocalDbChecker.Models;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LocalDbChecker.Models
{
    public class WebApiData
    {
        private readonly string _mainPartOfUri;
        private const string jsonPart = "&json";
        private const string dateFormat = "yyyyMMdd";
        public WebApiData(string partOfUr)
        {
            _mainPartOfUri = partOfUr;
        }

        public async Task<List<ExchangeRate>> GetCurrencyExchangeRate(DateTime time)
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
                 webResponce = await streamReader.ReadToEndAsync();
            }
            var listOfCurrencies = JsonConvert.DeserializeObject<List<ExchangeRate>>(webResponce);
            return listOfCurrencies;
        }

        public async Task<List<ExchangeRate>>GetCurrencyExchangeRate()
        {
            return await GetCurrencyExchangeRate(DateTime.Today);
        }
    }
}
