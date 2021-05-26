using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Models
{
    public static class Constants
    {
        public const string dbConnectionString = "Data Source = TWINGO; Initial Catalog = TestDb; Integrated Security = True; Connect Timeout = 30; Encrypt = False;TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
        public const string ApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date=";
            }
}
