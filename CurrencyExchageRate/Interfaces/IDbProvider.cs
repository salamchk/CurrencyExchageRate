using CurrencyExchageRate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDbProvider : IDataProvider
    {
        void SaveRates(List<ExchangeRate> rates);
    }
}
