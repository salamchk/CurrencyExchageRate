using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalDbChecker
{
    public class Rate
    {
        private static IApiProvider _fromApiProvider;
        private static IDbProvider _toDbProvider;
        private List<Task> _tasks = new List<Task>();
        private object _locker = new object();

        public Rate(IApiProvider fromApiProvider, IDbProvider toDbProvider)
        {
            _fromApiProvider = fromApiProvider;
            _toDbProvider = toDbProvider;
        }

        public void GetRates(DateTime date, int countDaysFromDate)
        {
            for (DateTime currentDate = date.AddDays(-countDaysFromDate); currentDate < date; currentDate = currentDate.AddDays(1))
            {
                var datetime = currentDate.Date;
                _tasks.Add(Task.Run(() => Save(datetime)));
            }
            Task.WhenAll(_tasks).Wait();
        }

        private void Save(DateTime currentDate)
        {
            var date = currentDate;
            var rates = new List<ExchangeRate>();
            lock (_locker)
                rates = _toDbProvider.GetCurrencyExchangeRate(date);
            if (rates == null || rates.Count <= 0)
                //Get rates from Api
                rates = _fromApiProvider.GetCurrencyExchangeRate(date);
            if (rates != null && rates.Count > 0)
            {
                //Save rates in database
                lock (_locker)
                    _toDbProvider.SaveRates(rates);
            }
        }
    }
}
