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
        private static string _connectionString;
        private static string _uri;

        private static List<Task> _tasks = new List<Task>();

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("In DBParser");
            var dataMap = context.JobDetail.JobDataMap;
            //Get data from context
            _connectionString = dataMap.GetString("connnectionString");
            _uri = dataMap.GetString("uri");
            var date = dataMap.GetDateTime("date");
            //Create providers for updating database
            GetRates(date);
        }

        private static void GetRates(DateTime date)
        {
            var semaphore = new Semaphore(30, 30);
            for (DateTime currentDate = date.AddDays(-30); currentDate < date; currentDate = currentDate.AddDays(1))
            {
                _tasks.Add(new Task(() => Save(currentDate)));
            }
            Task.WaitAll(_tasks.ToArray());
            _tasks.Clear();
        }


        private static void Save(object currentDate)
        {
            var dbProvider = new DbDataProvider(_connectionString);
            var apiProvider = new WebApiData(_uri);
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
