using CurrencyExchageRate.Interfaces;
using CurrencyExchageRate.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.DBControllers
{
    public class DbController : IDbProvider
    {
        public ISession Session { get; }

        public List<ExchangeRate> GetCurrencyExchangeRate()
        {
            throw new NotImplementedException();
        }
    }
}
