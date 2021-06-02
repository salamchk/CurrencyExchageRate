using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.Extensions.Logging;
using CurrencyRateLibrary.Interfaces;
using CurrencyRateLibrary.Models;

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

            var FakesData = new List<ExchangeRate>{
                new ExchangeRate(),
                new ExchangeRate(),
                new ExchangeRate()
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
              Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());
            apiProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());


            //act
            var result = controller.ExchangeRate(DateTime.Today.AddDays(3));

            //assert
            Assert.IsNull(result);
        }
    }
}