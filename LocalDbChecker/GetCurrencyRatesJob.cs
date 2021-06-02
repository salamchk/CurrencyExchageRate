using CurrencyRateLibrary.DB;
using CurrencyRateLibrary.WebClientBank;
using Quartz;
using System;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class GetCurrencyRatesJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("In DBParser");
            var dataMap = context.JobDetail.JobDataMap;
            //Get data from context
            var connectionString = dataMap.GetString("connnectionString");
            var uri = dataMap.GetString("uri");
            var date = dataMap.GetDateTime("date");
            //Create providers for updating database
            GetRates(connectionString, uri, date);
        }

        private static void GetRates(string connectionString, string uri, DateTime date)
        {
            var dbProvider = new DbDataProvider(connectionString);
            var apiProvider = new WebApiData(uri);

            var rates = dbProvider.GetCurrencyExchangeRate(date);
            if (rates != null && rates.Count > 0)
            {
            }
            else
            {
                //Get rates from Api
                rates = apiProvider.GetCurrencyExchangeRate(date);
                if (rates != null && rates.Count > 0)
                {
                    //Save rates in database
                    dbProvider.SaveRates(rates);
                }
            }
        }
    }
}
