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
                DbScheduler.Start(connectionString, uri);
            }
                catch (Exception)
                {
                    throw;
                }

        }
    }
}
