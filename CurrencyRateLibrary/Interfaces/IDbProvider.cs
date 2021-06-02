using CurrencyRateLibrary.Models;
using NHibernate;
using System.Collections.Generic;

namespace CurrencyRateLibrary.Interfaces
{
    public interface IDbProvider : IDataProvider
    {
        public ISession Session { get; }
        void SaveRates(List<ExchangeRate> rates);
    }
}
