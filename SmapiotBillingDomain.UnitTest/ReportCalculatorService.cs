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
                        new Request() { SubscriptionId = Guid.Empty.ToString() },
                        new Request() { SubscriptionId = specificId }
                    }
                });

            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void RequestsOfSubscription_ForSpecificId_GetOneWithSubscriptionId()
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
                        new Request() { SubscriptionId = Guid.Empty.ToString() },
                        new Request() { SubscriptionId = specificId }
                    }
                });

            Assert.AreEqual(result.First().SubscriptionId, specificId);
        }

        [TestMethod]
        public void CalculateCostPerServices_NullParameter_ThrowsArgumentNullException()
        {
            const string specificId = "BBCED7DB-6972-46D4-B6CF-D2C733E4B23D";
            var reportServiceMock = new Mock<IRequestCounterService>();
            var service = new ReportCalculatorService(reportServiceMock.Object);

            bool thrown = false;
            try
            {
                var result = service.CalculateCostPerServices(null);
            }
            catch (ArgumentNullException)
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void CalculateCostPerServices_OnAList_ShouldReturnACollectionOfPrices()
        {
            var reportServiceMock = new Mock<IRequestCounterService>();
            var service = new ReportCalculatorService(reportServiceMock.Object);

            var result = service.CalculateCostPerServices(new List<Request>());

            Assert.AreNotEqual(result, null);
        }

        [TestMethod]
        public void CalculateCostPerServices_OnAListOfWithOneItem_GivesBackAFilledInCostForThatService()
        {
            const string serviceName = "asd";
            var reportServiceMock = new Mock<IRequestCounterService>();
            var service = new ReportCalculatorService(reportServiceMock.Object);

            var results = service.CalculateCostPerServices(
                new List<Request>()
                {
                    new Request()
                    {
                        ServiceName = serviceName,
                    }
                });

            CostPerService result = results.First();

            Assert.AreEqual(result.ServiceName, serviceName);
            Assert.AreEqual(result.NumberOfRequests, 1);
            Assert.IsTrue(result.TotalPrice > 0);
        }

        [TestMethod]
        public void GetPricingForService_OnAnyServiceName_ReturnsAPositivePrice()
        {
            var reportServiceMock = new Mock<IRequestCounterService>();
            var service = new ReportCalculatorService(reportServiceMock.Object);

            decimal randomPrice = service.GetPricingForService(null);

            Assert.IsTrue(randomPrice > 0);
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
                                new Request() { SubscriptionId = Guid.Empty.ToString() }
                            }
                        }));

            var service = new ReportCalculatorService(reportServiceMock.Object);

            MonthlyReport result = service.CalculateReport(Guid.Empty.ToString(), 1, 1).Result;

            Assert.AreNotEqual(result, null);
        }

        [TestMethod]
        public void CalculateReport_AtleastOneRequestFound_ReturnsAReportFilledIn()
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
                            {
                                new Request() { SubscriptionId = specificId }
                            }
                        }));

            var service = new ReportCalculatorService(reportServiceMock.Object);

            MonthlyReport result = service.CalculateReport(specificId, 1, 1).Result;

            Assert.IsTrue(result.TotalNumberOfRequests > 0);
            Assert.AreEqual(result.SubscriptionId, specificId);
            Assert.IsTrue(result.StartDate.CompareTo(new DateTime(1, 1, 1)) == 0);
            Assert.IsTrue(result.EndDate.CompareTo(new DateTime(1, 1, 31)) == 0);
            Assert.AreNotEqual(result.Costs, null);
            Assert.AreEqual(result.EstimatedForRemaining, null);
        }


    }
}
