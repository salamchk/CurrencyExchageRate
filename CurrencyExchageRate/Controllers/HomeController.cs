
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
        //private readonly string _connectionString;
       // private readonly string _url;
        //private IDbProvider _dbProvider;
        //private IApiProvider _apiProvider;

       // public HomeController(ILogger<HomeController> logger, IConfiguration config, IDataProvider data, IApiProvider api)
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            //_connectionString = _configuration.GetConnectionString("dbConnectionString");
            //_url = _configuration.GetSection("ApiUrl").Value;
            //_dbProvider = new DataDB(_connectionString);
            //_apiProvider = new WebApiData(_url);
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
                var dbProvider = new DataDB(Constants.dbConnectionString);
                rates = dbProvider.GetCurrencyExchangeRate(date);
                if(rates!=null && rates.Count>0 )
                {
                    return rates;
                }
                else
                {
                    var apiProvider = new WebApiData(Constants.ApiUrl);
                    rates = apiProvider.GetCurrencyExchangeRate(date);
                    dbProvider.SaveRates(rates);
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
