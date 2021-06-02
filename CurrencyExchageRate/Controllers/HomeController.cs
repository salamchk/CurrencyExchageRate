using CurrencyExchageRate.Interfaces;
using CurrencyExchageRate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CurrencyExchageRate.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbProvider _dbProvider;
        private readonly IApiProvider _apiProvider;

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
            try
            {
               var rates = _dbProvider.GetCurrencyExchangeRate(date);
                if (rates != null && rates.Count > 0)
                {
                    return rates;
                }
                else
                {
                    rates = _apiProvider.GetCurrencyExchangeRate(date);
                    if (rates != null && rates.Count > 0)
                    {
                        _dbProvider.SaveRates(rates);
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
