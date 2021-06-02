using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class CurrencyRate
    {
        public virtual int ID { get; set; }

        public virtual int CurId { get; set; }

        public virtual float Rate { get; set; }
        public virtual int DateId { get; set; }


    }
}
