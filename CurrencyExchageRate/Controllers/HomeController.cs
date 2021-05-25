
using CurrencyExchageRate.DB;
using CurrencyExchageRate.Interfaces;
using CurrencyExchageRate.Models;
using DataLayer.Entities;
using DataLayer.Entities.Nhibernate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _url;
        private IDbProvider _dbProvider;
        private IApiProvider _apiProvider;


        public HomeController(ILogger<HomeController> logger, IConfiguration config, IDbProvider dbProvider, IApiProvider apiProvider)
        {
            _logger = logger;
            _dbProvider = dbProvider;
            _apiProvider = apiProvider;
            _configuration = config;
            _connectionString = _configuration.GetConnectionString("dbConnectionString");
            _url = _configuration.GetSection("ApiUrl").Value;
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
                List<ExchangeRate> rates;
                rates = _dbProvider.GetCurrencyExchangeRate(date);
                if(rates!=null && rates.Count>0 )
                {
                    return rates;
                }
                else
                {
                    rates = _apiProvider.GetCurrencyExchangeRate(date);
                    _dbProvider.SaveRates(rates);
                    return rates;
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
