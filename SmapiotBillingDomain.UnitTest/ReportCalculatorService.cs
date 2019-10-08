using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smapiot.Billing.Domain.Services;
using Smapiot.RequestCounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [TestMethod]
        public void RequestsOfSubscription_ForSpecificId_GetOne()
        {
            const string specificId = "BBCED7DB-6972-46D4-B6CF-D2C733E4B23D";

            var reportServiceMock = new Mock<IRequestCounterService>();

            var service = new ReportCalculatorService(reportServiceMock.Object);

            var result = service.RequestsOfSubscription(
                specificId, 
                new Requests()
                {
                    Requests1 = new List<Request>()
                        {
                            new Request() { Id = Guid.Empty.ToString() },
                            new Request() { Id = specificId }
                        }
                });

            Assert.AreEqual(result.Count(), 1);
        }
    }
}
