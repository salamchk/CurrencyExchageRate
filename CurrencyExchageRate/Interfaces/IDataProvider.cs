using CurrencyExchageRate.Models;
using NHibernate;
using System;
using System.Collections.Generic;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDataProvider
    {
        public ISession Session { get;}

        List<ExchangeRate> GetCurrencyExchangeRate(DateTime time);
        List<ExchangeRate> GetCurrencyExchangeRate();
    }
}
