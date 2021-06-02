using DataLayer.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mappings
{
    public class CurrencyDataMap : ClassMap<CurrencyData>
    {
        public CurrencyDataMap()
        {
            Id(x => x.ID);
            Map(x => x.cc);
            Map(x => x.r030);
            Map(x => x.txt);
        }
    }
}
