
using CurrencyExchageRate.Models;
using DataLayer.Entities;
using DataLayer.Entities.Nhibernate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Transform;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;

namespace CurrencyExchageRate.Controllers
{
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult ExchangeRate()
        {

            var session = NHibernateHelper.OpenSession();
            var currencyDatas = (from data in session.Query<CurrencyData>() select data).ToList();
            var date = (from exDate in session.Query<ExchangeDate>() where exDate.exDate == DateTime.Today select exDate).ToList().FirstOrDefault();
            if (date != null)
            {

                var ratesFromDB = (from curRate in session.Query<CurrencyRate>() where curRate.DateId == date.ID select curRate).ToList();
                var rates = ratesFromDB.Select(rate => new CurrencyExchangeRate()
                {
                    Rate = rate.Rate,
                    FullName = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.txt).SingleOrDefault(),
                    Indetifier = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.r030).SingleOrDefault(),
                    ShortName = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.cc).SingleOrDefault(),
                    ExchangeDate = date.exDate.ToShortDateString()
                }).ToList();
                return View(rates);
            }
            else
            {
                var uri = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
                var rates = WebExchangeRate.GetRates(uri);
                date = new ExchangeDate() { exDate = DateTime.Today };
                session.Save(date);

                if (currencyDatas.Count <= 0)
                {
                    currencyDatas = rates.Select(rate => new CurrencyData
                    {
                        cc = rate.ShortName,
                        txt = rate.FullName,
                        r030 = rate.Indetifier
                    }).ToList();
                    foreach (var item in currencyDatas)
                        session.Save(item);
                }
                var ratesToDb = rates.Select(rate =>
                new CurrencyRate()
                {
                    Rate = rate.Rate,
                    DateId = date.ID,
                    CurId = currencyDatas.Where(data => data.r030 == rate.Indetifier).Select(data => data.ID).Single()
                });
                foreach (var item in ratesToDb)
                    session.Save(item);

                return View(rates);
            }
        }

        [HttpPost]
        public ContentResult ExchangeRate(DateTime date, string currency)
        {
            CurrencyExchangeRate selectedCurrency;
            var uri =
            $"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={date.ToString("yyyyMMdd")}&json";
            var session = NHibernateHelper.OpenSession();
            var currencyDatas = (from data in session.Query<CurrencyData>() select data).ToList();
            var dateFromDb = (from exDate in session.Query<ExchangeDate>() where exDate.exDate == date select exDate).ToList().FirstOrDefault();
            if (dateFromDb != null)
            {

                var ratesFromDB = (from curRate in session.Query<CurrencyRate>() where curRate.DateId == dateFromDb.ID select curRate).ToList();
                var rates = ratesFromDB.Select(rate => new CurrencyExchangeRate()
                {
                    Rate = rate.Rate,
                    FullName = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.txt).SingleOrDefault(),
                    Indetifier = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.r030).SingleOrDefault(),
                    ShortName = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.cc).SingleOrDefault(),
                    ExchangeDate = dateFromDb.exDate.ToString()
                });
                selectedCurrency = rates.Where(rate => rate.ShortName == currency).SingleOrDefault();
            }
            else
            {
                var rates = WebExchangeRate.GetRates(uri);
                dateFromDb = new ExchangeDate() { exDate = date };
                session.Save(dateFromDb);
                if (currencyDatas.Count <= 0)
                {
                    currencyDatas = rates.Select(rate => new CurrencyData
                    {
                        cc = rate.ShortName,
                        txt = rate.FullName,
                        r030 = rate.Indetifier
                    }).ToList();
                    foreach (var item in currencyDatas)
                        session.Save(item);
                }
                var ratesToDb = rates.Select(rate =>
                new CurrencyRate()
                {
                    Rate = rate.Rate,
                    DateId = dateFromDb.ID,
                    CurId = currencyDatas.Where(data => data.r030 == rate.Indetifier).Select(data => data.ID).Single()
                });
                foreach (var item in ratesToDb)
                    session.Save(item);
                selectedCurrency =
                    WebExchangeRate.GetRates(uri).Where(cur => cur.ShortName == currency).FirstOrDefault();
            }
            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = $"<h1>{selectedCurrency.ExchangeDate}</h1><h2>1 {selectedCurrency.ShortName} = {selectedCurrency.Rate} UAH</h2>"
            };

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
