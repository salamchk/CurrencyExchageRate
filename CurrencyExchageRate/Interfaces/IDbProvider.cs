using CurrencyExchageRate.Models;
using NHibernate;
using System;
using System.Collections.Generic;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDbProvider
    {
        public ISession Session { get;}

        List<ExchangeRate> GetCurrencyExchangeRate();
        List<ExchangeRate> GetCurrencyExchangeRateByTime(DateTime time);
    }
}
