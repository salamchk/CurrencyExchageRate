using CurrencyExchageRate.Interfaces;
using CurrencyExchageRate.Models;
using DataLayer.Entities;
using DataLayer.Entities.Nhibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchageRate.DBControllers
{
    public class DbController : IDbProvider
    {
        public ISession Session { get; }

        public DbController()
        {
            Session = NHibernateHelper.OpenSession(ConfigurationManager.AppSettings["connectionString"]);
        }

        public List<ExchangeRate> GetCurrencyExchangeRate()
        {
            return GetCurrencyExchangeRateByTime(DateTime.Today);
        }

        public List<ExchangeRate> GetCurrencyExchangeRateByTime(DateTime time)
        {
            var currentDate = GetExchangeDate(time);
            if (currentDate != null)
            {
                var datasAboutCurrencies = GetCurrencyDatas();
                var currencyRates = GetCurrencyRates(currentDate.ID);

                return currencyRates.Select(rate => ConvertToExchangeRate(rate, currentDate.exDate,
                    datasAboutCurrencies.Where(data => data.ID == rate.CurId).SingleOrDefault())).ToList();
            }
            else
            {
                currentDate = new ExchangeDate() { exDate = time };
                SaveDateInDb(currentDate);
                var rates = WebExchangeRate.GetWebExchangeRate().GetRates(currentDate.exDate);
                var datasAboutCurrencies = GetCurrencyDatas();
                if (datasAboutCurrencies == null)
                {
                    SaveCurrencyData(rates.Select(rate => new CurrencyData()
                    {
                        cc = rate.ShortName,
                        r030 = rate.Indetifier,
                        txt = rate.FullName
                    }));
                }

                SaveCurrencyRates(rates.Select(rate => new CurrencyRate()
                {
                    Rate = rate.Rate,
                    CurId = datasAboutCurrencies.Where(data => data.r030 == rate.Indetifier).Select(data => data.ID).Single(),
                    DateId = currentDate.ID
                }));
                return rates;
            }

        }

        private void SaveCurrencyData(IEnumerable<CurrencyData> datas)
        {
            foreach (var data in datas)
                Session.Save(data);
        }

        private ExchangeRate ConvertToExchangeRate(CurrencyRate rate, DateTime date, CurrencyData data)
            => new ExchangeRate()
            {
                ExchangeDate = date.ToShortDateString(),
                FullName = data.txt,
                ShortName = data.cc,
                Indetifier = data.r030,
                Rate = rate.Rate
            };

        private List<CurrencyRate> GetCurrencyRates(int id)
            => (from curRate in Session.Query<CurrencyRate>()
                where curRate.DateId == id
                select curRate).ToList();

        private List<CurrencyData> GetCurrencyDatas()
            => (from data in Session.Query<CurrencyData>() select data).ToList();

        private void SaveDateInDb(ExchangeDate currentDate)
        {
            Session.Save(currentDate);
        }


        private ExchangeDate GetExchangeDate(DateTime time)
            => (from exDate in Session.Query<ExchangeDate>()
                where exDate.exDate == time
                select exDate).FirstOrDefault();




        private void SaveCurrencyRates(IEnumerable<CurrencyRate> rates)
        {
            foreach (var rate in rates)
                Session.Save(rate);
        }
    }
}
