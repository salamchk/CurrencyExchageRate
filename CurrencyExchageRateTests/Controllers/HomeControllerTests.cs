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
            //Arrange 
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();
            var mock = new Mock<ILogger<HomeController>>();

            var FakesData = new List<Models.ExchangeRate>{
                new Models.ExchangeRate(),
                new Models.ExchangeRate(),
                new Models.ExchangeRate()
            };
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger, dbProviderMock.Object, apiProviderMock.Object);
            dbProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.Is<DateTime>(date => date.Date <= DateTime.Today))).Returns(FakesData);
            apiProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.Is<DateTime>(date => date.Date <= DateTime.Today))).Returns(FakesData);

            //act
            var result = controller.ExchangeRate(DateTime.Today);

            //assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExchangeRateTestFailDatas()
        {
            //arrange
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();
            var mock = new Mock<ILogger<HomeController>>();
            ILogger<HomeController> logger = mock.Object;
            var controller = new HomeController(logger, dbProviderMock.Object, apiProviderMock.Object);

            dbProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
              Moq.It.IsAny<DateTime>())).Returns(new List<Models.ExchangeRate>());
            apiProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.IsAny<DateTime>())).Returns(new List<Models.ExchangeRate>());


            //act
            var result = controller.ExchangeRate(DateTime.Today.AddDays(3));

            //assert
            Assert.IsNull(result);
        }
    }
}