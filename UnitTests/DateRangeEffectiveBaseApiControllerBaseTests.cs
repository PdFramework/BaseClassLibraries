namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests
{
    using Contracts;
    using DataAccess.Contracts;
    using TestObjects;
    using Web.Http;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using System;
    using System.Data.Entity.Core;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;

    [TestClass]
    public class DateRangeEffectiveBaseApiControllerBaseTests
    {
        #region Mock Properties
        private Mock<IContractValidator<DateRangeEffectiveContractObject>> MockContractValidator { get; set; }
        private Mock<IDalBase<DateRangeEffectiveDtoObject, int>> MockDateRangeEffectiveObjectsDal { get; set; }
        private Mock<IIdPrincipal> MockUser { get; set; }
        private DateRangeEffectiveContractObjectsController DateRangeEffectiveContractObjectsController { get; set; }
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            MockContractValidator = new Mock<IContractValidator<DateRangeEffectiveContractObject>>();
            MockDateRangeEffectiveObjectsDal = new Mock<IDalBase<DateRangeEffectiveDtoObject, int>>();
            MockUser = new Mock<IIdPrincipal>();

            var config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(string.Format(CultureInfo.InvariantCulture, "http://localhost/v1/{0}", StaticTestValues.ControllerName)));
            var route = config.Routes.MapHttpRoute(StaticTestValues.GetDateRangeEffectiveContractObjectRouteName, string.Format(CultureInfo.InvariantCulture, "v1/{0}/{{id}}", StaticTestValues.ControllerName));
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { controller = StaticTestValues.ControllerName }));

            AutoMapperConfig.Configure();

            DateRangeEffectiveContractObjectsController = new DateRangeEffectiveContractObjectsController(MockDateRangeEffectiveObjectsDal.Object, AutoMapperConfig.Mapper, MockContractValidator.Object)
            {
                ControllerContext = new HttpControllerContext(config, routeData, request),
                Request = request,
                Configuration = config,
                Url = new UrlHelper(request),
                IdPrincipal = MockUser.Object
            };
            DateRangeEffectiveContractObjectsController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            DateRangeEffectiveContractObjectsController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
        }
        #endregion

        #region Get Tests
        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Get")]
        public async Task Given_AValidDateRangeEffectiveContractObjectId_When_GetIsInvoked_Then_OkStatusCodeShouldBeReturned()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(1)).ReturnsAsync(new DateRangeEffectiveDtoObject());

            var response = await (await DateRangeEffectiveContractObjectsController.Get(1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Get")]
        public async Task Given_AValidDateRangeEffectiveContractObjectId_When_GetIsInvoked_Then_TheDateRangeEffectiveContractObjectShouldBeReturned()
        {
            var dateRangeEffectiveDtoObject = new DateRangeEffectiveDtoObject
            {
                CreatedByUserId = StaticTestValues.CreatedByUserId1,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset1,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset1,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset1,
                Id = StaticTestValues.ValidId1,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId1,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset1,
                Property = StaticTestValues.ValidProperty1,
                VirtualProperty = StaticTestValues.ValidVirtualProperty1
            };

            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(StaticTestValues.ValidId1)).ReturnsAsync(dateRangeEffectiveDtoObject);

            var response = await (await DateRangeEffectiveContractObjectsController.Get(1)).ExecuteAsync(CancellationToken.None);
            var country = JsonConvert.DeserializeObject<DateRangeEffectiveContractObject>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(StaticTestValues.CreatedByUserId1, country.CreatedByUserId);
            Assert.AreEqual(StaticTestValues.CreatedOnDateTimeOffset1, country.CreatedOn);
            Assert.AreEqual(StaticTestValues.EffectiveEndDateTimeOffset1, country.EffectiveEndDate);
            Assert.AreEqual(StaticTestValues.EffectiveStartDateTimeOffset1, country.EffectiveStartDate);
            Assert.AreEqual(StaticTestValues.ValidId1, country.Id);
            Assert.AreEqual(StaticTestValues.LastUpdatedByUserId1, country.LastUpdatedByUserId);
            Assert.AreEqual(StaticTestValues.LastUpdatedOnDateTimeOffset1, country.LastUpdatedOn);
            Assert.AreEqual(StaticTestValues.ValidProperty1, country.Property);
            Assert.AreEqual(StaticTestValues.ValidVirtualProperty1, country.VirtualProperty);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Get")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObjectId_When_GetIsInvoked_Then_NotFoundStatusCodeShouldBeReturned()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            var response = await (await DateRangeEffectiveContractObjectsController.Get(0)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Get")]
        public async Task Given_AnyDateRangeEffectiveContractObjectId_When_GetIsInvoked_Then_DalReadMethodShouldOnlyBeInvokedOnce()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            await (await DateRangeEffectiveContractObjectsController.Get(0)).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Read(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region Post Tests
        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new [] { StaticTestValues.ContractValidatorErrorMessage1 });

            var response = await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_TheErrorMessagesShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new[] { StaticTestValues.ContractValidatorErrorMessage1, StaticTestValues.ContractValidatorErrorMessage2 });

            var response = await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual($"{StaticTestValues.ContractValidatorErrorMessage1}\n{StaticTestValues.ContractValidatorErrorMessage2}", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_DalCreateMethodShouldNotBeInvoked()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new [] { StaticTestValues.ContractValidatorErrorMessage1 });

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_ValidateContractMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new[] { StaticTestValues.ContractValidatorErrorMessage1 });
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockContractValidator.Verify(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidEffectiveDateRange_When_PostIsInvoked_Then_ValidateContractMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject { EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockContractValidator.Verify(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidEffectiveDateRange_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);

            var response = await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject { EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(ErrorMessages.InvalidEffectiveDateRange, responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidEffectiveDateRange_When_PostIsInvoked_Then_DalCreateMethodShouldNotBeInvoked()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject { EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_ValidateContractMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockContractValidator.Verify(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_CreatedResponseShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            var response = await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }


        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_TheDateRangeEffectiveContractObjectShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject
            {
                CreatedByUserId = StaticTestValues.CreatedByUserId1,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset1,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset1,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset1,
                Id = StaticTestValues.ValidId1,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId1,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset1,
                Property = StaticTestValues.ValidProperty1,
                VirtualProperty = StaticTestValues.ValidVirtualProperty1
            });

            var response = await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);
            var dateRangeEffectiveContractObject = JsonConvert.DeserializeObject<DateRangeEffectiveContractObject>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(StaticTestValues.CreatedByUserId1, dateRangeEffectiveContractObject.CreatedByUserId);
            Assert.AreEqual(StaticTestValues.CreatedOnDateTimeOffset1, dateRangeEffectiveContractObject.CreatedOn);
            Assert.AreEqual(StaticTestValues.EffectiveEndDateTimeOffset1, dateRangeEffectiveContractObject.EffectiveEndDate);
            Assert.AreEqual(StaticTestValues.EffectiveStartDateTimeOffset1, dateRangeEffectiveContractObject.EffectiveStartDate);
            Assert.AreEqual(StaticTestValues.ValidId1, dateRangeEffectiveContractObject.Id);
            Assert.AreEqual(StaticTestValues.LastUpdatedByUserId1, dateRangeEffectiveContractObject.LastUpdatedByUserId);
            Assert.AreEqual(StaticTestValues.LastUpdatedOnDateTimeOffset1, dateRangeEffectiveContractObject.LastUpdatedOn);
            Assert.AreEqual(StaticTestValues.ValidProperty1, dateRangeEffectiveContractObject.Property);
            Assert.AreEqual(StaticTestValues.ValidVirtualProperty1, dateRangeEffectiveContractObject.VirtualProperty);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_DalCreateMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_ResponseShouldContainTheUrlToGetTheCreatedContract()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject { Id = StaticTestValues.ValidId1 });

            var response = await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual($"http://localhost/v1/{StaticTestValues.ControllerName}/{StaticTestValues.ValidId1}?controller={StaticTestValues.ControllerName}", response.Headers.Location.ToString());
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Post")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PostIsInvoked_Then_DalCreateMethodShouldBeInvokedWithUpdatedAuditInformation()
        {
            MockUser.SetupGet(m => m.Id).Returns(StaticTestValues.CreatedByUserId2);
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Create(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Post(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Create(It.Is<DateRangeEffectiveDtoObject>(c => c.LastUpdatedByUserId == null && c.LastUpdatedOn == null && c.CreatedByUserId == StaticTestValues.CreatedByUserId2 && c.CreatedOn > DateTimeOffset.UtcNow.AddSeconds(-1) && c.CreatedOn < DateTimeOffset.UtcNow.AddSeconds(1))));
        }
        #endregion

        #region Put Tests
        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new[] { StaticTestValues.ContractValidatorErrorMessage1 });

            var response = await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_TheErrorMessagesShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new[] { StaticTestValues.ContractValidatorErrorMessage1, StaticTestValues.ContractValidatorErrorMessage2 });

            var response = await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual($"{StaticTestValues.ContractValidatorErrorMessage1}\n{StaticTestValues.ContractValidatorErrorMessage2}", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_DalUpdateMethodShouldNotBeInvoked()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new[] { StaticTestValues.ContractValidatorErrorMessage1 });

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_ValidateContractMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new[] { StaticTestValues.ContractValidatorErrorMessage1 });
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockContractValidator.Verify(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidEffectiveDateRange_When_PutIsInvoked_Then_ValidateContractMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject { EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockContractValidator.Verify(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidEffectiveDateRange_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);

            var response = await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject { EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(ErrorMessages.InvalidEffectiveDateRange, responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidEffectiveDateRange_When_PutIsInvoked_Then_DalUpdateMethodShouldNotBeInvoked()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject { EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_ValidateContractMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockContractValidator.Verify(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_OkResponseShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            var response = await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_TheDateRangeEffectiveContractObjectShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject
            {
                CreatedByUserId = StaticTestValues.CreatedByUserId1,
                CreatedOn = StaticTestValues.CreatedOnDateTimeOffset1,
                EffectiveEndDate = StaticTestValues.EffectiveEndDateTimeOffset1,
                EffectiveStartDate = StaticTestValues.EffectiveStartDateTimeOffset1,
                Id = StaticTestValues.ValidId1,
                LastUpdatedByUserId = StaticTestValues.LastUpdatedByUserId1,
                LastUpdatedOn = StaticTestValues.LastUpdatedOnDateTimeOffset1,
                Property = StaticTestValues.ValidProperty1,
                VirtualProperty = StaticTestValues.ValidVirtualProperty1
            });

            var response = await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);
            var dateRangeEffectiveContractObject = JsonConvert.DeserializeObject<DateRangeEffectiveContractObject>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(StaticTestValues.CreatedByUserId1, dateRangeEffectiveContractObject.CreatedByUserId);
            Assert.AreEqual(StaticTestValues.CreatedOnDateTimeOffset1, dateRangeEffectiveContractObject.CreatedOn);
            Assert.AreEqual(StaticTestValues.EffectiveEndDateTimeOffset1, dateRangeEffectiveContractObject.EffectiveEndDate);
            Assert.AreEqual(StaticTestValues.EffectiveStartDateTimeOffset1, dateRangeEffectiveContractObject.EffectiveStartDate);
            Assert.AreEqual(StaticTestValues.ValidId1, dateRangeEffectiveContractObject.Id);
            Assert.AreEqual(StaticTestValues.LastUpdatedByUserId1, dateRangeEffectiveContractObject.LastUpdatedByUserId);
            Assert.AreEqual(StaticTestValues.LastUpdatedOnDateTimeOffset1, dateRangeEffectiveContractObject.LastUpdatedOn);
            Assert.AreEqual(StaticTestValues.ValidProperty1, dateRangeEffectiveContractObject.Property);
            Assert.AreEqual(StaticTestValues.ValidVirtualProperty1, dateRangeEffectiveContractObject.VirtualProperty);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_DalUpdateMethodShouldOnlyBeInvokedOnce()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_ADateRangeEffectiveContractObjectWithAnInvalidId_When_PutIsInvoked_Then_NotFoundStatusShouldBeReturned()
        {
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.Is<DateRangeEffectiveDtoObject>(c => c.Id == StaticTestValues.InvalidId1))).ThrowsAsync(new ObjectNotFoundException());

            var response = await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject { Id = StaticTestValues.InvalidId1 })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Put")]
        public async Task Given_AValidDateRangeEffectiveContractObject_When_PutIsInvoked_Then_DalUpdateMethodShouldBeInvokedWithUpdatedAuditInformation()
        {
            MockUser.SetupGet(m => m.Id).Returns(StaticTestValues.LastUpdatedByUserId2.Value);
            MockContractValidator.Setup(m => m.ValidateContract(It.IsAny<DateRangeEffectiveContractObject>())).ReturnsAsync(new string[0]);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>())).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.Put(new DateRangeEffectiveContractObject())).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.Is<DateRangeEffectiveDtoObject>(c => c.LastUpdatedByUserId == StaticTestValues.LastUpdatedByUserId2 && c.LastUpdatedOn > DateTimeOffset.UtcNow.AddSeconds(-1) && c.LastUpdatedOn < DateTimeOffset.UtcNow.AddSeconds(1))));
        }
        #endregion

        #region SoftDelete Tests
        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi SoftDelete")]
        public async Task Given_AValidDateRangeEffectiveContractObjectId_When_SoftDeleteIsInvoked_Then_OkStatusCodeShouldBeReturned()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(StaticTestValues.ValidId1)).ReturnsAsync(new DateRangeEffectiveDtoObject());

            var response = await (await DateRangeEffectiveContractObjectsController.SoftDelete(StaticTestValues.ValidId1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi SoftDelete")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObjectId_When_SoftDeleteIsInvoked_Then_NotFoundStatusCodeShouldBeReturned()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(StaticTestValues.InvalidId1)).ThrowsAsync(new ObjectNotFoundException());

            var response = await (await DateRangeEffectiveContractObjectsController.SoftDelete(StaticTestValues.InvalidId1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi SoftDelete")]
        public async Task Given_AnyDateRangeEffectiveContractObjectId_When_SoftDeleteIsInvoked_Then_DalReadMethodShouldOnlyBeInvokedOnce()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(It.IsAny<int>())).ThrowsAsync(new ObjectNotFoundException());

            await (await DateRangeEffectiveContractObjectsController.SoftDelete(StaticTestValues.InvalidId1)).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Read(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi SoftDelete")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObjectId_When_SoftDeleteIsInvoked_Then_DalUpdateMethodShouldNotBeInvoked()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            await (await DateRangeEffectiveContractObjectsController.SoftDelete(0)).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi SoftDelete")]
        public async Task Given_AValidDateRangeEffectiveContractObjectId_When_SoftDeleteIsInvoked_Then_DalUpdateMethodShouldOnlyBeInvokedOnce()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(StaticTestValues.ValidId1)).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.SoftDelete(StaticTestValues.ValidId1)).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.IsAny<DateRangeEffectiveDtoObject>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi SoftDelete")]
        public async Task Given_AValidDateRangeEffectiveContractObjectId_When_SoftDeleteIsInvoked_Then_DalUpdateMethodShouldBeInvokedWithUpdatedAuditInformation()
        {
            MockUser.SetupGet(m => m.Id).Returns(StaticTestValues.LastUpdatedByUserId2.Value);
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Read(StaticTestValues.ValidId1)).ReturnsAsync(new DateRangeEffectiveDtoObject());

            await (await DateRangeEffectiveContractObjectsController.SoftDelete(StaticTestValues.ValidId1)).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Update(It.Is<DateRangeEffectiveDtoObject>(c => c.LastUpdatedByUserId == StaticTestValues.LastUpdatedByUserId2 && c.LastUpdatedOn > DateTimeOffset.UtcNow.AddSeconds(-1) && c.LastUpdatedOn < DateTimeOffset.UtcNow.AddSeconds(1) && c.EffectiveEndDate > DateTimeOffset.UtcNow.AddSeconds(-1) && c.EffectiveEndDate < DateTimeOffset.UtcNow.AddSeconds(1))));
        }
        #endregion

        #region Delete Tests
        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Delete")]
        public async Task Given_AValidDateRangeEffectiveContractObjectId_When_DeleteIsInvoked_Then_NoContentStatusCodeShouldBeReturned()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Delete(StaticTestValues.ValidId1)).Returns(Task.Factory.StartNew(() => { }));

            var response = await (await DateRangeEffectiveContractObjectsController.Delete(StaticTestValues.ValidId1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Delete")]
        public async Task Given_AnInvalidDateRangeEffectiveContractObjectId_When_DeleteIsInvoked_Then_NotFoundStatusCodeShouldBeReturned()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Delete(StaticTestValues.InvalidId1)).Throws(new ObjectNotFoundException());

            var response = await (await DateRangeEffectiveContractObjectsController.Delete(StaticTestValues.InvalidId1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("DateRangeEffectiveContractObjectsApi Delete")]
        public async Task Given_AnyDateRangeEffectiveContractObjectId_When_DeleteIsInvoked_Then_DalDeleteMethodShouldOnlyBeInvokedOnce()
        {
            MockDateRangeEffectiveObjectsDal.Setup(m => m.Delete(It.IsAny<int>())).Throws(new ObjectNotFoundException());

            await (await DateRangeEffectiveContractObjectsController.Delete(StaticTestValues.InvalidId1)).ExecuteAsync(CancellationToken.None);

            MockDateRangeEffectiveObjectsDal.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
        }
        #endregion
    }
}
