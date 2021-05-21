using CurrencyExchageRate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDataBankApiProvider
    {
        List<ExchangeRate> GetRates(DateTime time);
    }
}
