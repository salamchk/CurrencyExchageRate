using CurrencyRateLibrary.DB;
using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.WebClientBank;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocalDbChecker
{

    /// I can't create Action for Tasks dynamically, because Action is created the same as it was.
    /// So in this case, I can create many static actions and put them to a list of tasks 
    /// but from my point of view it is  not the best way to solve this problem.
    /// In other hand, I can create some static Tasks with different actions and wait 
    /// until they are completed and after that I can get other actions. But I think it's bad way also.
    /// The best way I founded for this solution is to create one Task, 
    /// wait until the work is ended and after that to create the next Task
    /// </summary>
    public class GetCurrencyRatesJob : IJob
    {
        private static string _connectionString;
        private static string _uri;

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
                var task = Task.Run(() => Save(currentDate, fromApiProvider, toDbProvider));
                task.Wait();
            }
        }

        public static void Save(DateTime currentDate, IApiProvider fromApiProvider, IDbProvider toDbProvider)
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

