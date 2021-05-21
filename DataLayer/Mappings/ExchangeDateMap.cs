using DataLayer.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mappings
{
    public class ExchangeDateMap : ClassMap<ExchangeDate>
    {
        public ExchangeDateMap()
        {
            Id(x => x.ID);
            Map(x => x.exDate);
            HasMany(x => x.CurrencyRates).Inverse();
        } 
    }
}
