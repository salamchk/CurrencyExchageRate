using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class CurrencyData
    {

        public virtual int ID { get; set; }
        //contains a three-digit currency code according to the Kl_r030 currency classifier
        public virtual int r030 { get; set; }
        //Full Name of Currency
        public virtual string txt { get; set; }

        //currency identifier
        public virtual string cc { get; set; }

    }
}
