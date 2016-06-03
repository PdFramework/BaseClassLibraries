namespace UnitTests
{
    using Apis;
    using Apis.Config;
    using Apis.Controllers;
    using Contracts;
    using DataAccessContracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using System;
    using System.Data.Entity.Core;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;

    [TestClass]
    public class CountriesControllerTests
    {
        #region Mock Properties
        private Mock<ICountriesDal> MockCountriesDal { get; set; }
        private Mock<IIdPrincipal> MockUser { get; set; }
        private CountriesController CountriesController { get; set; }
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            MockCountriesDal = new Mock<ICountriesDal>();
            MockUser = new Mock<IIdPrincipal>();

            var config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/v1/countries");
            var route = config.Routes.MapHttpRoute("GetCountry", "v1/countries/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { controller = "Countries" }));

            CountriesController = new CountriesController(MockCountriesDal.Object)
            {
                ControllerContext = new HttpControllerContext(config, routeData, request),
                Request = request,
                Configuration = config,
                Url = new UrlHelper(request),
                IdPrincipal = MockUser.Object
            };
            CountriesController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            CountriesController.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            AutoMapperConfig.Configure();
        }
        #endregion

        #region Get Tests
        [TestMethod]
        [TestCategory("CountriesApi Get")]
        public async Task Given_AValidCountryId_When_GetIsInvoked_Then_OkStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.Read(1)).ReturnsAsync(new CountryDto());

            var response = await (await CountriesController.Get(1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Get")]
        public async Task Given_AValidCountryId_When_GetIsInvoked_Then_TheCountryShouldBeReturned()
        {
            var countryDto = new CountryDto
            {
                Alpha2Code = "Alpha2Code",
                Alpha3Code = "Alpha3Code",
                CreatedByUserId = 1,
                CreatedOn = DateTimeOffset.MinValue.AddDays(1),
                EffectiveEndDate = DateTimeOffset.MaxValue,
                EffectiveStartDate = DateTimeOffset.MinValue.AddDays(1),
                FullName = "FullName",
                Id = 2,
                Iso3166Code = 3,
                LastUpdatedByUserId = 4,
                LastUpdatedOn = DateTimeOffset.MaxValue.AddDays(-1),
                PhoneNumberRegex = "PhoneNumberRegex",
                PostalCodeRegex = "PostalCodeRegex",
                ShortName = "ShortName"
            };

            MockCountriesDal.Setup(m => m.Read(1)).ReturnsAsync(countryDto);

            var response = await (await CountriesController.Get(1)).ExecuteAsync(CancellationToken.None);
            var country = JsonConvert.DeserializeObject<Country>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Alpha2Code", country.Alpha2Code);
            Assert.AreEqual("Alpha3Code", country.Alpha3Code);
            Assert.AreEqual(1, country.CreatedByUserId);
            Assert.AreEqual(DateTimeOffset.MinValue.AddDays(1), country.CreatedOn);
            Assert.AreEqual(DateTimeOffset.MaxValue, country.EffectiveEndDate);
            Assert.AreEqual(DateTimeOffset.MinValue.AddDays(1), country.EffectiveStartDate);
            Assert.AreEqual("FullName", country.FullName);
            Assert.AreEqual(2, country.Id);
            Assert.AreEqual(3, country.Iso3166Code);
            Assert.AreEqual(4, country.LastUpdatedByUserId);
            Assert.AreEqual(DateTimeOffset.MaxValue.AddDays(-1), country.LastUpdatedOn);
            Assert.AreEqual("PhoneNumberRegex", country.PhoneNumberRegex);
            Assert.AreEqual("PostalCodeRegex", country.PostalCodeRegex);
            Assert.AreEqual("ShortName", country.ShortName);
        }

        [TestMethod]
        [TestCategory("CountriesApi Get")]
        public async Task Given_AnInvalidCountryId_When_GetIsInvoked_Then_NotFoundStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            var response = await (await CountriesController.Get(0)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Get")]
        public async Task Given_AnyCountryId_When_GetIsInvoked_Then_CountriesDalReadMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            await (await CountriesController.Get(0)).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Read(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region Post Tests
        #region Validate
        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAName_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullName_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueName_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            var response = await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PostIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            var response = await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAName_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's name can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's abbreviation can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullName_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's name can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's abbreviation can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueName_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            var response = await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country name needs to be unique.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            var response = await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country abbreviation needs to be unique.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithAnInvalidEffectiveDateRange_When_PostIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName", EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's effective date range is invalid. EffectiveEndDate must be null or greater than the EffectiveStartDate.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithAnInvalidEffectiveDateRange_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code", EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithAnInvalidEffectiveDateRange_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code", EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAName_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullName_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueName_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAName_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullName_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Post(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueName_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            await (await CountriesController.Post(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Never);
        }
        #endregion

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_AValidCountry_When_PostIsInvoked_Then_CreatedResponseShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            var response = await (await CountriesController.Post(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_AValidCountry_When_PostIsInvoked_Then_TheCountryShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto
            {
                Alpha2Code = "Alpha2Code",
                Alpha3Code = "Alpha3Code",
                CreatedByUserId = 1,
                CreatedOn = DateTimeOffset.MinValue.AddDays(1),
                EffectiveEndDate = DateTimeOffset.MaxValue,
                EffectiveStartDate = DateTimeOffset.MinValue.AddDays(1),
                FullName = "FullName",
                Id = 2,
                Iso3166Code = 3,
                LastUpdatedByUserId = 4,
                LastUpdatedOn = DateTimeOffset.MaxValue.AddDays(-1),
                PhoneNumberRegex = "PhoneNumberRegex",
                PostalCodeRegex = "PostalCodeRegex",
                ShortName = "ShortName"
            });

            var response = await (await CountriesController.Post(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            var country = JsonConvert.DeserializeObject<Country>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Alpha2Code", country.Alpha2Code);
            Assert.AreEqual("Alpha3Code", country.Alpha3Code);
            Assert.AreEqual(1, country.CreatedByUserId);
            Assert.AreEqual(DateTimeOffset.MinValue.AddDays(1), country.CreatedOn);
            Assert.AreEqual(DateTimeOffset.MaxValue, country.EffectiveEndDate);
            Assert.AreEqual(DateTimeOffset.MinValue.AddDays(1), country.EffectiveStartDate);
            Assert.AreEqual("FullName", country.FullName);
            Assert.AreEqual(2, country.Id);
            Assert.AreEqual(3, country.Iso3166Code);
            Assert.AreEqual(4, country.LastUpdatedByUserId);
            Assert.AreEqual(DateTimeOffset.MaxValue.AddDays(-1), country.LastUpdatedOn);
            Assert.AreEqual("PhoneNumberRegex", country.PhoneNumberRegex);
            Assert.AreEqual("PostalCodeRegex", country.PostalCodeRegex);
            Assert.AreEqual("ShortName", country.ShortName);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_AValidCountry_When_PostIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await (await CountriesController.Post(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_AValidCountry_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await (await CountriesController.Post(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_AValidCountry_When_PostIsInvoked_Then_ResponseShouldContainTheUrlToGetTheCreatedCountry()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto { Id = 2 });

            var response = await (await CountriesController.Post(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            
            Assert.AreEqual("http://localhost/v1/countries/2?controller=Countries", response.Headers.Location.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Post")]
        public async Task Given_AValidCountry_When_PostIsInvoked_Then_CountriesDalCreateMethodShouldBeInvokedWithUpdatedAuditInformation()
        {
            MockUser.SetupGet(m => m.Id).Returns(10);
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await (await CountriesController.Post(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Create(It.Is<CountryDto>(c => c.LastUpdatedByUserId == null && c.LastUpdatedOn == null && c.CreatedByUserId == 10 && c.CreatedOn > DateTimeOffset.UtcNow.AddSeconds(-1) && c.CreatedOn < DateTimeOffset.UtcNow.AddSeconds(1))));
        }
        #endregion

        #region Put Tests
        #region Validate
        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAName_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullName_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueName_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            var response = await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PutIsInvoked_Then_BadRequestStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            var response = await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAName_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's name can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's abbreviation can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullName_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's name can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's abbreviation can't be blank.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueName_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            var response = await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country name needs to be unique.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            var response = await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country abbreviation needs to be unique.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAName_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullName_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueName_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAName_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { Alpha2Code = string.Empty, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullName_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { FullName = null, Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithANullAlpha2Code_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { Alpha2Code = null, FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueName_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == "FullName"))).ReturnsAsync(new[] { new CountryDto { FullName = "FullName" } });

            await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == "Alpha2Code"))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = "Alpha2Code" } });

            await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithAnInvalidEffectiveDateRange_When_PutIsInvoked_Then_TheReasonShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);

            var response = await (await CountriesController.Put(new Country { Alpha2Code = "Alpha2Code", FullName = "FullName", EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);
            dynamic responseContent = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Country's effective date range is invalid. EffectiveEndDate must be null or greater than the EffectiveStartDate.", responseContent.Message.ToString());
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithAnInvalidEffectiveDateRange_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code", EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithAnInvalidEffectiveDateRange_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await (await CountriesController.Put(new Country { FullName = string.Empty, Alpha2Code = "Alpha2Code", EffectiveStartDate = DateTimeOffset.MaxValue, EffectiveEndDate = DateTimeOffset.MinValue })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }
        #endregion

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_AValidCountry_When_PutIsInvoked_Then_OkResponseShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Update(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            var response = await (await CountriesController.Put(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_AValidCountry_When_PutIsInvoked_Then_TheCountryShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Update(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto
            {
                Alpha2Code = "Alpha2Code",
                Alpha3Code = "Alpha3Code",
                CreatedByUserId = 1,
                CreatedOn = DateTimeOffset.MinValue.AddDays(1),
                EffectiveEndDate = DateTimeOffset.MaxValue,
                EffectiveStartDate = DateTimeOffset.MinValue.AddDays(1),
                FullName = "FullName",
                Id = 2,
                Iso3166Code = 3,
                LastUpdatedByUserId = 4,
                LastUpdatedOn = DateTimeOffset.MaxValue.AddDays(-1),
                PhoneNumberRegex = "PhoneNumberRegex",
                PostalCodeRegex = "PostalCodeRegex",
                ShortName = "ShortName"
            });

            var response = await (await CountriesController.Put(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);
            var country = JsonConvert.DeserializeObject<Country>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual("Alpha2Code", country.Alpha2Code);
            Assert.AreEqual("Alpha3Code", country.Alpha3Code);
            Assert.AreEqual(1, country.CreatedByUserId);
            Assert.AreEqual(DateTimeOffset.MinValue.AddDays(1), country.CreatedOn);
            Assert.AreEqual(DateTimeOffset.MaxValue, country.EffectiveEndDate);
            Assert.AreEqual(DateTimeOffset.MinValue.AddDays(1), country.EffectiveStartDate);
            Assert.AreEqual("FullName", country.FullName);
            Assert.AreEqual(2, country.Id);
            Assert.AreEqual(3, country.Iso3166Code);
            Assert.AreEqual(4, country.LastUpdatedByUserId);
            Assert.AreEqual(DateTimeOffset.MaxValue.AddDays(-1), country.LastUpdatedOn);
            Assert.AreEqual("PhoneNumberRegex", country.PhoneNumberRegex);
            Assert.AreEqual("PostalCodeRegex", country.PostalCodeRegex);
            Assert.AreEqual("ShortName", country.ShortName);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_AValidCountry_When_PutIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Update(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await (await CountriesController.Put(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_AValidCountry_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Update(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await (await CountriesController.Put(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_ACountryWithAnInvalidId_When_PutIsInvoked_Then_NotFoundStatusShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Update(It.Is<CountryDto>(c => c.Id == -1))).ThrowsAsync(new ObjectNotFoundException());

            var response = await (await CountriesController.Put(new Country { Id = -1, FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Put")]
        public async Task Given_AValidCountry_When_PutIsInvoked_Then_CountriesDalUpdateMethodShouldBeInvokedWithUpdatedAuditInformation()
        {
            MockUser.SetupGet(m => m.Id).Returns(10);
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Update(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await (await CountriesController.Put(new Country { FullName = "FullName", Alpha2Code = "Alpha2Code" })).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.Is<CountryDto>(c => c.LastUpdatedByUserId == 10 && c.LastUpdatedOn > DateTimeOffset.UtcNow.AddSeconds(-1) && c.LastUpdatedOn < DateTimeOffset.UtcNow.AddSeconds(1))));
        }
        #endregion

        #region SoftDelete Tests
        [TestMethod]
        [TestCategory("CountriesApi SoftDelete")]
        public async Task Given_AValidCountryId_When_SoftDeleteIsInvoked_Then_OkStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.Read(1)).ReturnsAsync(new CountryDto());

            var response = await (await CountriesController.SoftDelete(1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi SoftDelete")]
        public async Task Given_AnInvalidCountryId_When_SoftDeleteIsInvoked_Then_NotFoundStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            var response = await (await CountriesController.SoftDelete(0)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi SoftDelete")]
        public async Task Given_AnyCountryId_When_SoftDeleteIsInvoked_Then_CountriesDalReadMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.Read(It.IsAny<int>())).ThrowsAsync(new ObjectNotFoundException());

            await (await CountriesController.SoftDelete(0)).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Read(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi SoftDelete")]
        public async Task Given_AnInvalidCountryId_When_SoftDeleteIsInvoked_Then_CountriesDalUpdateMethodShouldNotBeInvoked()
        {
            MockCountriesDal.Setup(m => m.Read(0)).ThrowsAsync(new ObjectNotFoundException());

            await (await CountriesController.SoftDelete(0)).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Never);
        }

        [TestMethod]
        [TestCategory("CountriesApi SoftDelete")]
        public async Task Given_AValidCountryId_When_SoftDeleteIsInvoked_Then_CountriesDalUpdateMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.Read(1)).ReturnsAsync(new CountryDto());

            await (await CountriesController.SoftDelete(1)).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountriesApi SoftDelete")]
        public async Task Given_AValidCountryId_When_SoftDeleteIsInvoked_Then_CountriesDalUpdateMethodShouldBeInvokedWithUpdatedAuditInformation()
        {
            MockUser.SetupGet(m => m.Id).Returns(10);
            MockCountriesDal.Setup(m => m.Read(1)).ReturnsAsync(new CountryDto());

            await (await CountriesController.SoftDelete(1)).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Update(It.Is<CountryDto>(c => c.LastUpdatedByUserId == 10 && c.LastUpdatedOn > DateTimeOffset.UtcNow.AddSeconds(-1) && c.LastUpdatedOn < DateTimeOffset.UtcNow.AddSeconds(1) && c.EffectiveEndDate > DateTimeOffset.UtcNow.AddSeconds(-1) && c.EffectiveEndDate < DateTimeOffset.UtcNow.AddSeconds(1))));
        }
        #endregion

        #region Delete Tests
        [TestMethod]
        [TestCategory("CountriesApi Delete")]
        public async Task Given_AValidCountryId_When_DeleteIsInvoked_Then_NoContentStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.Delete(1)).Returns(Task.Factory.StartNew(() => { }));

            var response = await (await CountriesController.Delete(1)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Delete")]
        public async Task Given_AnInvalidCountryId_When_DeleteIsInvoked_Then_NotFoundStatusCodeShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.Delete(0)).Throws(new ObjectNotFoundException());

            var response = await (await CountriesController.Delete(0)).ExecuteAsync(CancellationToken.None);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("CountriesApi Delete")]
        public async Task Given_AnyCountryId_When_DeleteIsInvoked_Then_CountriesDalDeleteMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.Delete(It.IsAny<int>())).Throws(new ObjectNotFoundException());

            await (await CountriesController.Delete(0)).ExecuteAsync(CancellationToken.None);

            MockCountriesDal.Verify(m => m.Delete(It.IsAny<int>()), Times.Once);
        }
        #endregion
    }
}
