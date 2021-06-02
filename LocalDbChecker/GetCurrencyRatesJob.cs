using CurrencyRateLibrary.DB;
using CurrencyRateLibrary.WebClientBank;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class GetCurrencyRatesJob : IJob
    {
        private static string connectionString;
        private static string uri;

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("In DBParser");
            var dataMap = context.JobDetail.JobDataMap;
            //Get data from context
            connectionString = dataMap.GetString("connnectionString");
            uri = dataMap.GetString("uri");
            var date = dataMap.GetDateTime("date");
            //Create providers for updating database
            GetRates(date);
        }

        private static void GetRates(DateTime date)
        {
            var semaphore = new Semaphore(30, 30);
            for (DateTime currentDate = date.AddDays(-30); currentDate < date; currentDate = currentDate.AddDays(1))
            {
                semaphore.WaitOne();
                Thread thread = new Thread(new ParameterizedThreadStart(Save));
                thread.Start(currentDate);
                semaphore.Release();
            }
        }


        private static void Save(object currentDate)
        {
            var dbProvider = new DbDataProvider(connectionString);
            var apiProvider = new WebApiData(uri);
            var date = (DateTime)currentDate;
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
