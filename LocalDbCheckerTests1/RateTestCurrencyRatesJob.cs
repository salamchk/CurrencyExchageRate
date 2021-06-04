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
    public class RateTestCurrencyRatesJob
    {
        [TestMethod()]
        public void GetRatesTest()
        {
            //arrange
            var dbProviderMock = new Mock<IDbProvider>();
            var apiProviderMock = new Mock<IApiProvider>();

            dbProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
              Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());
            dbProviderMock.Setup(obj=>obj.SaveRates(Moq.It.IsAny<List<ExchangeRate>>()));
            apiProviderMock.Setup(obj => obj.GetCurrencyExchangeRate(
                Moq.It.IsAny<DateTime>())).Returns(new List<ExchangeRate>());
            try
            {
                //act
                var rate = new Rate(apiProviderMock.Object, dbProviderMock.Object);
                rate.GetRates(DateTime.Today, 30);
            }
            catch(Exception e)
            {
                //assert
                Assert.Fail(e.Message);
            }
        }
    }
}