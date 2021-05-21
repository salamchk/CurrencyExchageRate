﻿using DataLayer.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.Interfaces
{
    public interface IDbProvider
    {
        public ISession Session { get; set; }

        List<CurrencyExchageRate> GetCurrencyExchangeRate();
    }
}
