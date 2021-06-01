using LocalDbChecker.DB;
using LocalDbChecker.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class DbParser : IJob
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
            var dbProvider = new DbDataProvider(connectionString);
            var apiProvider = new WebApiData(uri);

            var rates = dbProvider.GetCurrencyExchangeRate(date);
            if (rates != null && rates.Count > 0)
            {
            }
            else
            {
                //Get rates from Api
                rates = await apiProvider.GetCurrencyExchangeRate(date);
                if (rates != null && rates.Count > 0)
                {
                    //Save rates in database
                    dbProvider.SaveRates(rates);
                }
            }
        }
    }
}
