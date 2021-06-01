using LocalDbChecker.Models;
using System;
using System.Collections.Generic;

namespace LocalDbChecker.Interfaces
{
    public interface IDataProvider
    {
        List<ExchangeRate> GetCurrencyExchangeRate(DateTime time);
        List<ExchangeRate> GetCurrencyExchangeRate();
    }
}
