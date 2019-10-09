using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Smapiot.Billing.Domain.Models;
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
        public void CalculateReport_FutureMonth_ReturnsNull()
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

        [TestMethod]
        public void CalculateReport_RequestsNull_ThrowsArgumentNullException()
        {
            const string specificId = "BBCED7DB-6972-46D4-B6CF-D2C733E4B23D";

            var reportServiceMock = new Mock<IRequestCounterService>();

            var service = new ReportCalculatorService(reportServiceMock.Object);

            bool thrown = false;
            try
            {
                var result = service.RequestsOfSubscription(
                        specificId,
                        null);
            }
            catch (ArgumentNullException)
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void CalculateReport_NoRequestFound_ReturnsNull()
        {
            const string specificId = "BBCED7DB-6972-46D4-B6CF-D2C733E4B23D";

            var reportServiceMock = new Mock<IRequestCounterService>();
            reportServiceMock
                .Setup(moq =>
                    moq._api_requests_year_month_getAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(
                    Task.FromResult<Requests>(
                        new Requests()
                        {
                            Requests1 = new List<Request>()
                        }));

            var service = new ReportCalculatorService(reportServiceMock.Object);

            MonthlyReport result = service.CalculateReport(specificId, 0, 0).Result;

            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void CalculateReport_AtleastOneRequestFound_ReturnsAReportCreated()
        {
            var reportServiceMock = new Mock<IRequestCounterService>();
            reportServiceMock
                .Setup(moq =>
                    moq._api_requests_year_month_getAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(
                    Task.FromResult<Requests>(
                        new Requests()
                        {
                            Requests1 = new List<Request>()
                        {
                            new Request() { Id = Guid.Empty.ToString() }
                        }
                        }));

            var service = new ReportCalculatorService(reportServiceMock.Object);

            MonthlyReport result = service.CalculateReport(Guid.Empty.ToString(), 0, 0).Result;

            Assert.AreNotEqual(result, null);
        }
    }
}
