namespace IntegrationTests.Non_SpecFlow
{
    using ApiClients;
    using Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    [TestClass]
    public class CountriesTests
    {
        private ICountriesClient CountriesClient { get; }
        private TestData TestData { get; }

        public CountriesTests()
        {
            CountriesClient = new CountriesApiClient();
            TestData = new TestData();
        }

        [TestMethod]
        public async Task TestCRUDHappyPathForCountries()
        {
            var country = await Given_ACountry_When_CreateCountryIsInvoked_Then_TheCountryIsReturned();
            await Given_ACountryInTheDatabase_When_GetCountryIsInvokedWithTheCountryId_Then_TheCountryIsReturned(country);
            country = await Given_ACountryInTheDatabaseWithSomeUpdatedProperties_When_UpdateCountryIsInvokedWithTheUpdatedCountry_Then_TheUpdatedCountryIsReturned(country);
            await Given_ACountryInTheDatabase_When_GetCountryIsInvokedWithTheCountryId_Then_TheCountryIsReturned(country);
            await Given_ACountryIdInTheDatabase_When_DeleteCountryIsInvokedWithTheCountryId_Then_TheCountryIsDeleted(country.Id);
        }

        private async Task<Country> Given_ACountry_When_CreateCountryIsInvoked_Then_TheCountryIsReturned()
        {
            var us = new Country
            {
                Alpha2Code = TestData.UsCountryAlpha2Code,
                CreatedByUserId = TestData.CountryCreatedById,
                CreatedOn = TestData.NonServiceGeneratedCountryCreatedOn,
                Id = TestData.NonServiceGeneratedCountryId,
                LastUpdatedByUserId = TestData.CountryLastUpdatedById,
                LastUpdatedOn = TestData.NonServiceGeneratedCountryLastUpdatedOn,
                FullName = TestData.UsCountryName,
                PhoneNumberRegex = TestData.UsPhoneNumberRegex,
                PostalCodeRegex = TestData.UsPostalCodeRegex
            };

            try
            {
                var usReturned = await CountriesClient.CreateCountry(us);

                Assert.AreEqual(TestData.UsCountryAlpha2Code, usReturned.Alpha2Code);
                Assert.AreEqual(TestData.CountryCreatedById, usReturned.CreatedByUserId);
                Assert.AreEqual(TestData.UsCountryName, usReturned.FullName);
                Assert.AreNotEqual(TestData.NonServiceGeneratedCountryCreatedOn, usReturned.CreatedOn);
                Assert.AreNotEqual(TestData.NonServiceGeneratedCountryId, usReturned.Id);
                Assert.IsNull(usReturned.LastUpdatedByUserId);
                Assert.IsNull(usReturned.LastUpdatedOn);
                Assert.AreEqual(TestData.UsPhoneNumberRegex, usReturned.PhoneNumberRegex);
                Assert.AreEqual(TestData.UsPostalCodeRegex, usReturned.PostalCodeRegex);

                us = usReturned;
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}\n{e.StackTrace}");
            }

            return us;
        }

        private async Task Given_ACountryInTheDatabase_When_GetCountryIsInvokedWithTheCountryId_Then_TheCountryIsReturned(Country country)
        {
            try
            {
                var countryReturned = await CountriesClient.GetCountry(country.Id);

                Assert.AreEqual(country.Alpha2Code, countryReturned.Alpha2Code);
                Assert.AreEqual(country.CreatedByUserId, countryReturned.CreatedByUserId);
                Assert.AreEqual(country.CreatedOn, countryReturned.CreatedOn);
                Assert.AreEqual(country.Id, countryReturned.Id);
                Assert.AreEqual(country.LastUpdatedByUserId, countryReturned.LastUpdatedByUserId);
                Assert.AreEqual(country.LastUpdatedOn, countryReturned.LastUpdatedOn);
                Assert.AreEqual(country.FullName, countryReturned.FullName);
                Assert.AreEqual(country.PhoneNumberRegex, countryReturned.PhoneNumberRegex);
                Assert.AreEqual(country.PostalCodeRegex, countryReturned.PostalCodeRegex);
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}\n{e.StackTrace}");
            }
        }

        private async Task<Country> Given_ACountryInTheDatabaseWithSomeUpdatedProperties_When_UpdateCountryIsInvokedWithTheUpdatedCountry_Then_TheUpdatedCountryIsReturned(Country country)
        {
            try
            {
                country.Alpha2Code = TestData.CaCountryAbbreviation;
                country.FullName = TestData.CaCountryName;
                country.PhoneNumberRegex = TestData.CaPhoneNumberRegex;
                country.PostalCodeRegex = TestData.CaPostalCodeRegex;
                country.CreatedByUserId = TestData.CountryCreatedByIdForUpdate;
                country.CreatedOn = TestData.NonServiceGeneratedCountryCreatedOn;
                country.LastUpdatedByUserId = TestData.CountryLastUpdatedById;
                country.LastUpdatedOn = TestData.NonServiceGeneratedCountryLastUpdatedOn;

                var countryReturned = await CountriesClient.UpdateCountry(country);

                Assert.AreEqual(TestData.CaCountryAbbreviation, countryReturned.Alpha2Code);
                Assert.AreEqual(TestData.CaCountryName, countryReturned.FullName);
                Assert.AreEqual(TestData.CaPhoneNumberRegex, countryReturned.PhoneNumberRegex);
                Assert.AreEqual(TestData.CaPostalCodeRegex, countryReturned.PostalCodeRegex);
                Assert.AreNotEqual(TestData.CountryCreatedByIdForUpdate, countryReturned.CreatedByUserId);
                Assert.AreNotEqual(TestData.NonServiceGeneratedCountryCreatedOn, countryReturned.CreatedOn);
                Assert.AreEqual(country.Id, countryReturned.Id);
                Assert.AreEqual(TestData.CountryLastUpdatedById, countryReturned.LastUpdatedByUserId);
                Assert.AreNotEqual(TestData.NonServiceGeneratedCountryLastUpdatedOn, countryReturned.LastUpdatedOn);

                country = countryReturned;
            }
            catch (Exception e)
            {
                Assert.Fail($"{e.Message}\n{e.StackTrace}");
            }

            return country;
        }

        private async Task Given_ACountryIdInTheDatabase_When_DeleteCountryIsInvokedWithTheCountryId_Then_TheCountryIsDeleted(int countryId)
        {
            try
            {
                await CountriesClient.DeleteCountry(countryId);
                var softDeletedAddress = await CountriesClient.GetCountry(countryId);
                Assert.IsTrue(softDeletedAddress.EffectiveEndDate > DateTime.UtcNow.AddSeconds(-5) && softDeletedAddress.EffectiveEndDate <= DateTime.UtcNow);

                var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings["Countries.Endpoint"], "v1/Countries", countryId.ToString(), "force"));

                using (var client = new HttpClient())
                {
                    using (var response = await client.DeleteAsync(uri))
                    {
                        if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.Content.ToString());
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Assert.Fail($"{e.Message}\n{e.StackTrace}");
            }

            try
            {
                await CountriesClient.GetCountry(countryId);
                Assert.Fail($"Address id: {countryId} was found after it should have been deleted.");
            }
            catch (HttpRequestException)
            {
            }
        }
    }
}
