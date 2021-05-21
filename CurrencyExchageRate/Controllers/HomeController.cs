
using CurrencyExchageRate.DBControllers;
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
            DbController db = new DbController();


                return View(db.GetCurrencyExchangeRate());
        }

        [HttpPost]
        public ContentResult ExchangeRate(DateTime date, string currency)
        {
            DbController db = new DbController();
            var rates = db.GetCurrencyExchangeRateByTime(date);
            return new ContentResult();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
