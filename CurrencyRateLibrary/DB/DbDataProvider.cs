using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.Models;
using DataLayer.Entities;
using DataLayer.Entities.Nhibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyRateLibrary.DB
{
    public class DbDataProvider : IDbProvider
    {
        public ISession Session { get; private set; }

        private readonly object _locker = new object();
        public DbDataProvider(string connectionString)
        {
            Session = NHibernateHelper.OpenSession(connectionString);
        }

        public List<ExchangeRate> GetCurrencyExchangeRate()
        {
            return GetCurrencyExchangeRate(DateTime.Today);
        }

        public List<ExchangeRate> GetCurrencyExchangeRate(DateTime time)
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
                return null;
            }
        }

        private List<CurrencyData> SaveCurrencyData(List<CurrencyData> datas)
        {
            for (int i = 0; i < datas.Count(); i++)
            {
                Session.Save(datas[i]);
            }
            return datas;
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

        public void SaveRates(List<ExchangeRate> rates)
        {
            lock (_locker)
            {
                try
                {
                    if (DateTime.TryParseExact(rates.First().ExchangeDate,
                        "dd.MM.yyyy",
                        System.Globalization.CultureInfo.CurrentCulture,
                            System.Globalization.DateTimeStyles.None, out DateTime date))
                    {
                        using var transaction = Session.BeginTransaction();
                        try
                        {
                            var currentDate = new ExchangeDate() { exDate = date };
                            SaveDateInDb(currentDate);
                            var dataOfRates = GetCurrencyDatas();

                            if (dataOfRates == null || dataOfRates.Count <= 0)
                                dataOfRates = SaveCurrencyData(rates.Select(rate => new CurrencyData()
                                {
                                    cc = rate.ShortName,
                                    r030 = rate.Indetifier,
                                    txt = rate.FullName
                                }).ToList()).ToList();

                            var absentData = rates.Where(rate => !dataOfRates.Select(data => data.cc).Contains(rate.ShortName)).Select(rate => new CurrencyData()
                            {
                                cc = rate.ShortName,
                                r030 = rate.Indetifier,
                                txt = rate.FullName
                            });

                            if (absentData.Count() > 0)
                            {
                                dataOfRates.AddRange(absentData);
                                SaveCurrencyData(dataOfRates);
                            }

                            SaveCurrencyRates(rates.Select(rate => new CurrencyRate()
                            {
                                Rate = rate.Rate,
                                CurId = dataOfRates.Where(data => data.r030 == rate.Indetifier).Select(data => data.ID).FirstOrDefault(),
                                DateId = currentDate.ID
                            }));

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            transaction.Rollback();
                        }
                    }
                    else throw new Exception("Can't parse time");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
