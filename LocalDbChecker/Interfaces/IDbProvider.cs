using LocalDbChecker.Models;
using NHibernate;
using System.Collections.Generic;

namespace LocalDbChecker.Interfaces
{
    public interface IDbProvider : IDataProvider
    {
        public ISession Session { get; }
        void SaveRates(List<ExchangeRate> rates);
    }
}
