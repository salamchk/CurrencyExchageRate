
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
using System.Collections.Generic;
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
        /// <summary>
        /// For Change ur on Exchange2:
        /// [HttpGet("Exchange2")]
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public ActionResult ExchangeRate()
        {
            try
            {
                DataDB db = new DataDB();
                List<ExchangeRate> rates;
                rates = db.GetCurrencyExchangeRate();
                if (rates != null && rates.Count > 0)
                    return View(db.GetCurrencyExchangeRate());
                else return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogCritical($"LogCritical {e.Message}");
                return BadRequest();
            }
        }


        [HttpPost("")]
        public ActionResult ExchangeRate(string date)
        {
            try
            {
                DataDB db = new DataDB();
                List<ExchangeRate> rates;
                if (date != null && DateTime.TryParse(date, out DateTime selectedDate))
                {
                    rates = db.GetCurrencyExchangeRate(selectedDate);
                }
                else
                {
                    rates = db.GetCurrencyExchangeRate();
                }
                if (rates != null && rates.Count > 0)
                    return View(rates);
                else return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
