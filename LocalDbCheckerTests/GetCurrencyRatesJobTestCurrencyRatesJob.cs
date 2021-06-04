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
            //arrange
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();
            
            dbProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
              Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());
            apiProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());
            //act
            var rate = new Rate(apiProviderMock.Object, dbProviderMock.Object);
            rate.GetRates(DateTime.Today, 30);
            //assert
        }

        [TestMethod()]
        public void SaveTest2()
        {
            //arrange
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();
            //act
            var rate = new Rate(apiProviderMock.Object, dbProviderMock.Object);
            rate.GetRates(DateTime.Today, 30);

            //assert
        }
    }
}