namespace UnitTests
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using Apis.Config;
    using Apis.ContractValidators;
    using Contracts;
    using DataAccessContracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class CountryValidatorTests
    {
        private const string Alpha2Code = "Alpha2Code";
        private const string FullName = "FullName";

        #region Mock Properties
        private Mock<ICountriesDal> MockCountriesDal { get; set; }
        private IContractValidator<Country> CountryValidator { get; set; }
        #endregion

        #region Initialize
        [TestInitialize]
        public void Initialize()
        {
            MockCountriesDal = new Mock<ICountriesDal>();
            AutoMapperConfig.Configure();

            CountryValidator = new CountryValidator(MockCountriesDal.Object, AutoMapperConfig.Mapper);
        }
        #endregion

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAName_When_ValidateContractIsInvoked_Then_ErrorMessageShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var errors = await CountryValidator.ValidateContract(new Country { FullName = string.Empty, Alpha2Code = Alpha2Code});

            Assert.AreEqual(errors.First(), ErrorMessages.CountryNameMissing);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_ValidateContractIsInvoked_Then_ErrorMessageShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            var errors = await CountryValidator.ValidateContract(new Country { Alpha2Code = string.Empty, FullName = FullName });

            Assert.AreEqual(errors.First(), ErrorMessages.CountryAbbreviationMissing);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithANullName_When_ValidateContractIsInvoked_Then_ErrorMessageShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            var errors = await CountryValidator.ValidateContract(new Country { FullName = null, Alpha2Code = Alpha2Code });

            Assert.AreEqual(errors.First(), ErrorMessages.CountryNameMissing);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithANullAlpha2Code_When_ValidateContractIsInvoked_Then_ErrorMessageShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            var errors = await CountryValidator.ValidateContract(new Country { Alpha2Code = null, FullName = FullName });

            Assert.AreEqual(errors.First(), ErrorMessages.CountryAbbreviationMissing);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAUniqueName_When_ValidateContractIsInvoked_Then_ErrorMessageShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == FullName))).ReturnsAsync(new[] { new CountryDto { FullName = FullName } });

            var errors = await CountryValidator.ValidateContract(new Country { Alpha2Code = Alpha2Code, FullName = FullName });

            Assert.AreEqual(errors.First(), ErrorMessages.NonUniqueCountryName);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_ValidateContractIsInvoked_Then_ErrorMessageShouldBeReturned()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == Alpha2Code))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = Alpha2Code } });

            var errors = await CountryValidator.ValidateContract(new Country { Alpha2Code = Alpha2Code, FullName = FullName });

            Assert.AreEqual(errors.First(), ErrorMessages.NonUniqueCountryAbbreviation);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAName_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await CountryValidator.ValidateContract(new Country { FullName = string.Empty, Alpha2Code = Alpha2Code });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAnAlpha2Code_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == string.Empty))).ReturnsAsync(new CountryDto[0]);

            await CountryValidator.ValidateContract(new Country { Alpha2Code = string.Empty, FullName = FullName });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithANullName_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == null))).ReturnsAsync(new CountryDto[0]);

            await CountryValidator.ValidateContract(new Country { FullName = null, Alpha2Code = Alpha2Code });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithANullAlpha2Code_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == null))).ReturnsAsync(new CountryDto[0]);

            await CountryValidator.ValidateContract(new Country { Alpha2Code = null, FullName = FullName });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAUniqueName_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.FullName == FullName))).ReturnsAsync(new[] { new CountryDto { FullName = FullName } });

            await CountryValidator.ValidateContract(new Country { Alpha2Code = Alpha2Code, FullName = FullName });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_ACountryWithoutAUniqueAlpha2Code_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.Is<CountryDto>(c => c.Alpha2Code == Alpha2Code))).ReturnsAsync(new[] { new CountryDto { Alpha2Code = Alpha2Code } });

            await CountryValidator.ValidateContract(new Country { Alpha2Code = Alpha2Code, FullName = FullName });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("CountryValidator")]
        public async Task Given_AValidCountry_When_ValidateContractIsInvoked_Then_CountriesDalGetMatchingCountriesMethodShouldOnlyBeInvokedOnce()
        {
            MockCountriesDal.Setup(m => m.GetMatchingCountries(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto[0]);
            MockCountriesDal.Setup(m => m.Create(It.IsAny<CountryDto>())).ReturnsAsync(new CountryDto());

            await CountryValidator.ValidateContract(new Country { FullName = FullName, Alpha2Code = Alpha2Code });

            MockCountriesDal.Verify(m => m.GetMatchingCountries(It.IsAny<CountryDto>()), Times.Once);
        }
    }
}
