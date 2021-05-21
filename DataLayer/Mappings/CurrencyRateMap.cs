using DataLayer.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mappings
{
    public class CurrencyRateMap : ClassMap<CurrencyRate>
    {
        public CurrencyRateMap()
        {
            Id(x => x.ID);
            Map(x => x.CurId);
            Map(x => x.DateId);
            Map(x => x.Rate);

        }
    }
}
