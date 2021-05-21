
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

        /// <summary>
        /// Interfaces: 
        /// IDataProvider - work with data
        /// IApiProvider 
        /// </summary>
        /// <param name="logger"></param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        routing Exchange2
        [HttpGet]
        public ActionResult ExchangeRate()
        {
            //Add Error handling with logging
            var session = NHibernateHelper.OpenSession();
            var currencyDatas = (from data in session.Query<CurrencyData>() select data).ToList();
            var date = (from exDate in session.Query<ExchangeDate>() where exDate.exDate == DateTime.Today select exDate).ToList().FirstOrDefault();
            if (date != null)
            {

                var ratesFromDB = (from curRate in session.Query<CurrencyRate>() where curRate.DateId == date.ID select curRate).ToList();
                var rates = ratesFromDB.Select(rate => new CurrencyExchangeRate()
                {
                    rate = rate.Rate,
                    txt = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.txt).SingleOrDefault(),
                    r030 = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.r030).SingleOrDefault(),
                    cc = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.cc).SingleOrDefault(),
                    exchangedate = date.exDate.ToShortDateString()
                }).ToList();
                return View(rates);
            }
            else
            {
                // var books = linq.ToList();
                // const, config file
                var uri = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
                var rates = WebExchangeRate.GetRates(uri);
                date = new ExchangeDate() { exDate = DateTime.Today };
                session.Save(date);

                if (currencyDatas.Count <= 0)
                {
                    currencyDatas = rates.Select(rate => new CurrencyData
                    {
                        cc = rate.cc,
                        txt = rate.txt,
                        r030 = rate.r030
                    }).ToList();
                    foreach (var item in currencyDatas)
                        session.Save(item);
                }
                var ratesToDb = rates.Select(rate =>
                new CurrencyRate()
                {
                    Rate = rate.rate,
                    DateId = date.ID,
                    CurId = currencyDatas.Where(data => data.r030 == rate.r030).Select(data => data.ID).Single()
                });
                foreach (var item in ratesToDb)
                    session.Save(item);

                return View(rates);
            }
        }

        //Change ur to Exchange2
        [HttpPost]
        public ContentResult ExchangeRate(DateTime date, string currency)
        {
            CurrencyExchangeRate selectedCurrency;
            //const or config
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
                    rate = rate.Rate,
                    txt = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.txt).SingleOrDefault(),
                    r030 = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.r030).SingleOrDefault(),
                    cc = currencyDatas.Where(data => data.ID == rate.CurId).Select(data => data.cc).SingleOrDefault(),
                    exchangedate = dateFromDb.exDate.ToString()
                });
                selectedCurrency = rates.Where(rate => rate.cc == currency).SingleOrDefault();
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
                        cc = rate.cc,
                        txt = rate.txt,
                        r030 = rate.r030
                    }).ToList();
                    foreach (var item in currencyDatas)
                        session.Save(item);
                }
                var ratesToDb = rates.Select(rate =>
                new CurrencyRate()
                {
                    Rate = rate.rate,
                    DateId = dateFromDb.ID,
                    CurId = currencyDatas.Where(data => data.r030 == rate.r030).Select(data => data.ID).Single()
                });
                foreach (var item in ratesToDb)
                    session.Save(item);
                selectedCurrency =
                    WebExchangeRate.GetRates(uri).Where(cur => cur.cc == currency).FirstOrDefault();
            }

            //raws data - no html, use JS to format result
            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = $"<h1>{selectedCurrency.exchangedate}</h1><h2>1 {selectedCurrency.cc} = {selectedCurrency.rate} UAH</h2>"
            };

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
