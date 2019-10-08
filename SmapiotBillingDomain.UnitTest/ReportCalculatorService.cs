using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smapiot.Billing.Domain.Services;
using Smapiot.RequestCounter;
using System;

namespace SmapiotBillingDomain.UnitTest
{
    [TestClass]
    public class ReportCalculatorServiceTest
    {
        [TestMethod]
        public void CalculateReport_FutureMont_ReturnsNull()
        {
            var reportServiceMock = new Mock<IRequestCounterService>();
            var service = new ReportCalculatorService(reportServiceMock.Object);

            var result = service.CalculateReport(null, DateTime.MaxValue.Year, DateTime.MaxValue.Month).Result;

            Assert.IsNull(result);
        }


    }
}
