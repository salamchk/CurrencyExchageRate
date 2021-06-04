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
    public class GetCurrencyRatesJob : IJob
    {
        private static string _connectionString;
        private static string _uri;

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            //Get data from context
            _connectionString = dataMap.GetString("connnectionString");
            _uri = dataMap.GetString("uri");
            var date = dataMap.GetDateTime("date");

            var rate = new Rate(new WebApiDataProvider(_uri), new DbDataProvider(_connectionString));
            rate.GetRates(date, 30);
        }
    }
}

