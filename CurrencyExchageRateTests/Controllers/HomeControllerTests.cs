using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurrencyExchageRate.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using CurrencyExchageRate.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchageRate.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void ExchangeRateTest()
        {
            var dbProvidermock = new Mock<IDbProvider>();
            var apiProvidermock = new Mock<IApiProvider>();
            var mock = new Mock<ILogger<HomeController>>();

            var inMemorySettings = new Dictionary<string, string> {
                {"ApiUrl", "url"},
                 {"dbConnectionString", "connection"},
};
            ILogger<HomeController> logger = mock.Object;
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            //dbProvidermock.Setup(obj => obj.GetCurrencyExchangeRate(Moq.It.IsAny<DateTime>())).Returns(new List<Models.ExchangeRate>());
            //apiProvidermock.Setup(obj => obj.GetCurrencyExchangeRate(Moq.It.IsAny<DateTime>())).Returns(new List<Models.ExchangeRate>());
            var controller = new HomeController(logger, config);
            var result = controller.ExchangeRate(DateTime.Today);
            Assert.IsNotNull(result);
        }
    }
}