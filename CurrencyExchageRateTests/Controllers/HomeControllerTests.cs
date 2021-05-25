using Microsoft.VisualStudio.TestTools.UnitTesting;
using CurrencyExchageRate.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using CurrencyExchageRate.Interfaces;

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

            dbProvidermock.Setup(obj => obj.GetCurrencyExchangeRate(Moq.It.IsAny<DateTime>())).Returns(new List<Models.ExchangeRate>());
            apiProvidermock.Setup(obj => obj.GetCurrencyExchangeRate(Moq.It.IsAny < DateTime>())).Returns(new List<Models.ExchangeRate>());

            //Assert.Fail();
        }
    }
}