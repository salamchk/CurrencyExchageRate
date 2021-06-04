using CurrencyRateLibrary.DB;
using CurrencyRateLibrary.WebClientBank;
using Quartz;
using System;
using System.Configuration;

namespace LocalDbChecker
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Started");
            var connectionString = ConfigurationManager.ConnectionStrings["localDbString"].ConnectionString;
            var uri = ConfigurationManager.AppSettings["ApiUrl"];
            try
            {
                var rate = new Rate(new WebApiDataProvider(uri), new DbDataProvider(connectionString));
                rate.GetRates(DateTime.Today, 30);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
