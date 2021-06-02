using CurrencyExchageRate.Models;
using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CurrencyExchageRate.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbProvider _dbProvider;
        private readonly IApiProvider _apiProvider;

        public static List<ExchangeRate> Rates { get; set; }

        public HomeController(ILogger<HomeController> logger, IDbProvider dbProvider, IApiProvider apiProvider)
        {
            _logger = logger;
            _dbProvider = dbProvider;
            _apiProvider = apiProvider;
        }

        [HttpGet("")]
        public ActionResult ExchangeRate()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _logger.LogCritical($"LogCritical {e.Message}");
                return BadRequest();
            }
        }


        [HttpPost("{date}")]
        public List<ExchangeRate> ExchangeRate(DateTime date)
        {
            if(date.Date == DateTime.Today)
            {
                if (Rates != null && Rates.First().ExchangeDate == date.ToShortDateString()) return Rates;
            }
            try
            {
               var rates = _dbProvider.GetCurrencyExchangeRate(date);
                if (rates != null && rates.Count > 0)
                {
                    if (rates.First().ExchangeDate == DateTime.Today.ToShortDateString())
                        Rates = rates;
                    return rates;
                }
                else
                {
                    rates = _apiProvider.GetCurrencyExchangeRate(date);
                    if (rates != null && rates.Count > 0)
                    {
                        _dbProvider.SaveRates(rates);
                        if (rates.First().ExchangeDate == DateTime.Today.ToShortDateString())
                            Rates = rates;
                        return rates;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical($"LogCritical {e.Message}");
                return null;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
