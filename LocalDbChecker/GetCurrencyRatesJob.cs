using CurrencyRateLibrary.DB;
using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.WebClientBank;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalDbChecker
{
    public class GetCurrencyRatesJob : IJob
    {
        private static string _connectionString;
        private static string _uri;

        //private static List<Task> _tasks = new List<Task>();
        private static object _locker = new object();
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("In DBParser");
            var dataMap = context.JobDetail.JobDataMap;
            //Get data from context
            _connectionString = dataMap.GetString("connnectionString");
            _uri = dataMap.GetString("uri");
            var date = dataMap.GetDateTime("date");
            //Create providers for updating database
            GetRates(date, new WebApiData(_uri), new DbDataProvider(_connectionString));
        }

        public static void GetRates(DateTime date, IApiProvider fromApiProvider, IDbProvider toDbProvider)
        {

            for (DateTime currentDate = date.AddDays(-30); currentDate < date; currentDate = currentDate.AddDays(1))
            {
                var work = new Action(() => Save(currentDate, fromApiProvider, toDbProvider));
                var task = new Task((Action)work.Clone());
                task.Start();
                task.Wait();
            
             }
            //Task.WaitAll(_tasks.ToArray());

        }


        public static void Save(DateTime currentDate, IApiProvider fromApiProvider, IDbProvider toDbProvider)
        {
            lock (_locker)
            {
                var date = currentDate;
                var rates = toDbProvider.GetCurrencyExchangeRate(date);
                if (rates == null || rates.Count <= 0)
                    //Get rates from Api
                    rates = fromApiProvider.GetCurrencyExchangeRate(date);
                if (rates != null && rates.Count > 0)
                {
                    //Save rates in database
                    toDbProvider.SaveRates(rates);
                }
            }
        }
    }
}

