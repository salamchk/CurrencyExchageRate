using CurrencyExchageRate.Models;
using NHibernate;
using System.Collections.Generic;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDbProvider : IDataProvider
    {
        public ISession Session { get; }
        void SaveRates(List<ExchangeRate> rates);
    }
}
