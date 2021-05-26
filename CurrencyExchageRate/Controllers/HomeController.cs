﻿
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
        private IDbProvider _dbProvider;
        private IApiProvider _apiProvider;

        public HomeController(ILogger<HomeController> logger, IDbProvider data, IApiProvider api)
        {
            _logger = logger;
            _dbProvider = data;
            _apiProvider = api;
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
                List<ExchangeRate> rates;
                rates = _dbProvider.GetCurrencyExchangeRate(date);
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
