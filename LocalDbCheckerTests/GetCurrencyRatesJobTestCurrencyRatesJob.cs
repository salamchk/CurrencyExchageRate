using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalDbChecker;
using System;
using System.Collections.Generic;
using System.Text;
using CurrencyRateLibrary.Interfaces;
using Moq;
using CurrencyRateLibrary.Models;

namespace LocalDbChecker.Tests
{
    [TestClass()]
    public class GetCurrencyRatesJobTestCurrencyRatesJob
    {
        [TestMethod()]
        public void SaveTest()
        {
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();
            
            dbProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
              Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());
            apiProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());

            GetCurrencyRatesJob.GetRates(DateTime.Today, apiProviderMock.Object, dbProviderMock.Object);

        }

        [TestMethod()]
        public void SaveTest2()
        {
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();
            GetCurrencyRatesJob.GetRates(DateTime.Today, apiProviderMock.Object, dbProviderMock.Object);
        }
    }
}