using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Nhibernate
{
    public class NHibernateHelper
    {
        public static ISession OpenSession(string connectionString)
        {

            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.
                ConnectionString(connectionString).ShowSql())
                .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<CurrencyData>().AddFromAssemblyOf<CurrencyRate>()
                .AddFromAssemblyOf<ExchangeDate>()).ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
