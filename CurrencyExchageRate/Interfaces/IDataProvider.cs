using CurrencyExchageRate.Models;
using System;
using System.Collections.Generic;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDataProvider
    {
        List<ExchangeRate> GetCurrencyExchangeRate(DateTime time);
        List<ExchangeRate> GetCurrencyExchangeRate();
    }
}
