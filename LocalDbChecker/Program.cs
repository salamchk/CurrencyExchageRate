
using LocalDbChecker;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

            try
            {
                    //DbScheduler.Start();
                }
                catch (Exception)
                {
                    throw;
                }

        }
    }
}
