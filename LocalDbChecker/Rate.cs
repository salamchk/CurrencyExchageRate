using CurrencyRateLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDbChecker
{
    public class Rate
    {
        private static IApiProvider _fromApiProvider;
        private static IDbProvider _toDbProvider;

        public Rate(IApiProvider fromApiProvider, IDbProvider toDbProvider)
        {
            _fromApiProvider = fromApiProvider;
            _toDbProvider = toDbProvider;
        }

        public void GetRates(DateTime date, int countDaysFromDate)
        {
            for (DateTime currentDate = date.AddDays(-countDaysFromDate); currentDate < date; currentDate = currentDate.AddDays(1))
            {
                var task = Task.Run(() => Save(currentDate));
                task.Wait();
            }
        }

        private void Save(DateTime currentDate)
        {
            var date = currentDate;
            var rates = _toDbProvider.GetCurrencyExchangeRate(date);
            if (rates == null || rates.Count <= 0)
                //Get rates from Api
                rates = _fromApiProvider.GetCurrencyExchangeRate(date);
            if (rates != null && rates.Count > 0)
            {
                //Save rates in database
                _toDbProvider.SaveRates(rates);
            }
        }
    }
}
