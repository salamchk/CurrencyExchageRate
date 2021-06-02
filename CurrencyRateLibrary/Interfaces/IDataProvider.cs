using CurrencyRateLibrary.Models;
using System;
using System.Collections.Generic;

namespace CurrencyRateLibrary.Interfaces
{
    public interface IDataProvider
    {
        List<ExchangeRate> GetCurrencyExchangeRate(DateTime time);
        List<ExchangeRate> GetCurrencyExchangeRate();
    }
}
