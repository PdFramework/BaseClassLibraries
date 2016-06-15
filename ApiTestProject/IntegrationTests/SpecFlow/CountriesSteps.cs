namespace IntegrationTests.SpecFlow
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Contracts;

    using ApiClients;
    using Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class CountriesSteps
    {
        #region Setup
        private IList<Country> CountriesToCreate { get; set; }
        private IList<Country> TestCountriesCreated { get; set; }
        private Country CountryRecievedFromApi { get; set; }
        private Country CountryValuesToUpdate { get; set; }
        private ICountriesClient CountriesClient { get; }

        public CountriesSteps()
        {
            CountriesClient = new CountriesApiClient();
        }

        [BeforeScenario]
        public void SetupBeforeScenario()
        {
            if (TestCountriesCreated != null && TestCountriesCreated.Any()) Cleanup(TestCountriesCreated);

            CountriesToCreate = new List<Country>();
            TestCountriesCreated = new List<Country>();
            CountryRecievedFromApi = null;
            CountryValuesToUpdate = null;
        }
        #endregion

        #region Given
        [Given(@"the following countries")]
        public void Given_TheFollowingCountries(Table table)
        {
            foreach (var country in table.CreateSet<Country>())
            {
                CountriesToCreate.Add(country);
            }
        }

        [Given(@"the following countries exist in the data store")]
        public void Given_TheFollowingCountriesExistInTheDataStore(Table table)
        {
            CreateCountries(table.CreateSet<Country>());
        }
        #endregion

        #region When
        [When(@"post is invoked on the countries api")]
        public void When_PostIsInvokedOnTheCountriesApi()
        {
            CreateCountries(CountriesToCreate);
        }

        [When(@"get is invoked on the countries api with the country's id")]
        public void When_GetIsInvokdeOnTheCountriesApiWithTheCountrysId()
        {
            CountryRecievedFromApi = CountriesClient.GetCountry(TestCountriesCreated.First().Id).GetAwaiter().GetResult();
        }

        [When(@"update is invoked on the countries api with the following values")]
        public void When_UpdateIsInvokedOnTheCountriesApiWithTheFollowingValues(Table table)
        {
            var countryIdToUpdate = TestCountriesCreated.First().Id;
            CountryValuesToUpdate = table.CreateInstance<Country>();

            CountriesClient.UpdateCountry(new Country
            {
                Alpha2Code = CountryValuesToUpdate.Alpha2Code,
                Alpha3Code = CountryValuesToUpdate.Alpha3Code,
                Id = countryIdToUpdate,
                FullName = CountryValuesToUpdate.FullName,
                ShortName = CountryValuesToUpdate.ShortName,
                EffectiveEndDate = CountryValuesToUpdate.EffectiveEndDate,
                EffectiveStartDate = CountryValuesToUpdate.EffectiveStartDate,
                Iso3166Code = CountryValuesToUpdate.Iso3166Code,
                PhoneNumberRegex = CountryValuesToUpdate.PhoneNumberRegex,
                PostalCodeRegex = CountryValuesToUpdate.PostalCodeRegex
            }).GetAwaiter().GetResult();
        }

        [When(@"delete is invoked on the countries api with the country's id")]
        public void When_DeleteIsInvokedOnTheCountriesApiWithTheCountrysId()
        {
           CountriesClient.DeleteCountry(TestCountriesCreated.First().Id).GetAwaiter().GetResult();
        }

        [When(@"force delete is invoked on the countries api with the country's id")]
        public void When_ForceDeleteIsInvokdeOnTheCountriesApiWithTheCountrysId()
        {
            Cleanup(new[] { TestCountriesCreated.First() });
        }
        #endregion

        #region Then
        [Then(@"the country should be returned with an id")]
        public void Then_TheCountryShouldBeReturnedWithAnId()
        {
            foreach (var countryAdded in TestCountriesCreated)
            {
                var countryToAdd = CountriesToCreate.First(country => country.FullName == countryAdded.FullName);
                Assert.AreEqual(countryToAdd.Alpha2Code, countryAdded.Alpha2Code);
                Assert.AreEqual(countryToAdd.Alpha3Code, countryAdded.Alpha3Code);
                //TODO: figure out how to add a user for integration tests
                Assert.AreEqual(-365, countryAdded.CreatedByUserId);
                Assert.AreEqual(countryToAdd.EffectiveEndDate, countryAdded.EffectiveEndDate);
                Assert.AreEqual(countryToAdd.EffectiveStartDate, countryAdded.EffectiveStartDate);
                Assert.AreEqual(countryToAdd.FullName, countryAdded.FullName);
                Assert.AreEqual(countryToAdd.Iso3166Code, countryAdded.Iso3166Code);
                Assert.AreEqual(countryToAdd.PhoneNumberRegex, countryAdded.PhoneNumberRegex);
                Assert.AreEqual(countryToAdd.PostalCodeRegex, countryAdded.PostalCodeRegex);
                Assert.AreEqual(countryToAdd.ShortName, countryAdded.ShortName);

                Assert.AreNotEqual(countryToAdd.Id, countryAdded.Id);

                Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < countryAdded.CreatedOn.UtcDateTime && countryAdded.CreatedOn.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));

                Assert.IsNull(countryAdded.LastUpdatedByUserId);
                Assert.IsNull(countryAdded.LastUpdatedOn);
            }
        }

        [Then(@"the country should be returned")]
        public void Then_TheCountryShouldBeReturned()
        {
            var createdTestCountry = TestCountriesCreated.First();

            Assert.AreEqual(createdTestCountry.Alpha2Code, CountryRecievedFromApi.Alpha2Code);
            Assert.AreEqual(createdTestCountry.Alpha3Code, CountryRecievedFromApi.Alpha3Code);
            Assert.AreEqual(createdTestCountry.CreatedByUserId, CountryRecievedFromApi.CreatedByUserId);
            Assert.AreEqual(createdTestCountry.EffectiveEndDate, CountryRecievedFromApi.EffectiveEndDate);
            Assert.AreEqual(createdTestCountry.EffectiveStartDate, CountryRecievedFromApi.EffectiveStartDate);
            Assert.AreEqual(createdTestCountry.FullName, CountryRecievedFromApi.FullName);
            Assert.AreEqual(createdTestCountry.Iso3166Code, CountryRecievedFromApi.Iso3166Code);
            Assert.AreEqual(createdTestCountry.PhoneNumberRegex, CountryRecievedFromApi.PhoneNumberRegex);
            Assert.AreEqual(createdTestCountry.PostalCodeRegex, CountryRecievedFromApi.PostalCodeRegex);
            Assert.AreEqual(createdTestCountry.ShortName, CountryRecievedFromApi.ShortName);
            Assert.AreEqual(createdTestCountry.Id, CountryRecievedFromApi.Id);
            Assert.AreEqual(createdTestCountry.CreatedOn, CountryRecievedFromApi.CreatedOn.UtcDateTime);
            Assert.AreEqual(createdTestCountry.LastUpdatedByUserId, CountryRecievedFromApi.LastUpdatedByUserId);
            Assert.AreEqual(createdTestCountry.LastUpdatedOn, CountryRecievedFromApi.LastUpdatedOn);
        }

        [Then(@"the country should be updated")]
        public void Then_TheCountryShouldBeUpdated()
        {
            var createdCountry = TestCountriesCreated.First();
            CountryRecievedFromApi = CountriesClient.GetCountry(createdCountry.Id).GetAwaiter().GetResult();

            Assert.AreEqual(CountryValuesToUpdate.Alpha2Code, CountryRecievedFromApi.Alpha2Code);
            Assert.AreEqual(CountryValuesToUpdate.Alpha3Code, CountryRecievedFromApi.Alpha3Code);
            Assert.AreEqual(createdCountry.CreatedByUserId, CountryRecievedFromApi.CreatedByUserId);
            Assert.AreEqual(CountryValuesToUpdate.EffectiveEndDate, CountryRecievedFromApi.EffectiveEndDate);
            Assert.AreEqual(CountryValuesToUpdate.EffectiveStartDate, CountryRecievedFromApi.EffectiveStartDate);
            Assert.AreEqual(CountryValuesToUpdate.FullName, CountryRecievedFromApi.FullName);
            Assert.AreEqual(CountryValuesToUpdate.Iso3166Code, CountryRecievedFromApi.Iso3166Code);
            Assert.AreEqual(CountryValuesToUpdate.PhoneNumberRegex, CountryRecievedFromApi.PhoneNumberRegex);
            Assert.AreEqual(CountryValuesToUpdate.PostalCodeRegex, CountryRecievedFromApi.PostalCodeRegex);
            Assert.AreEqual(CountryValuesToUpdate.ShortName, CountryRecievedFromApi.ShortName);
            Assert.AreEqual(createdCountry.Id, CountryRecievedFromApi.Id);
            Assert.AreEqual(createdCountry.CreatedOn, CountryRecievedFromApi.CreatedOn.UtcDateTime);
            Assert.AreEqual(-365, CountryRecievedFromApi.LastUpdatedByUserId);
            Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < CountryRecievedFromApi.LastUpdatedOn.Value.UtcDateTime && CountryRecievedFromApi.LastUpdatedOn.Value.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));
        }

        [Then(@"the country should exist in the data store")]
        public void Then_TheCountryShouldExistInTheDataStore()
        {
            CountryRecievedFromApi = CountriesClient.GetCountry(TestCountriesCreated.First().Id).GetAwaiter().GetResult();
            Assert.IsNotNull(CountryRecievedFromApi);
        }

        [Then(@"the country's effective end date should be updated")]
        public void Then_TheCountrysEffectiveEndDateShouldBeUpdated()
        {
            Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < CountryRecievedFromApi.EffectiveEndDate.Value.UtcDateTime && CountryRecievedFromApi.EffectiveEndDate.Value.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));
        }

        [Then(@"the country should not exist in the data store")]
        public void Then_TheCountryShouldNotExistInTheDataStore()
        {
            try
            {
                CountriesClient.GetCountry(TestCountriesCreated.First().Id).GetAwaiter().GetResult();
                Assert.Fail();
            }
            catch (ApiInvokerException)
            {
                TestCountriesCreated.Remove(TestCountriesCreated.First());
            }
        }
        #endregion

        #region Cleanup
        [AfterScenario]
        public void CleanupAfterScenario()
        {
            Cleanup(TestCountriesCreated);
        }
        #endregion

        #region Helper methods
        private void CreateCountries(IEnumerable<Country> countriesToCreate)
        {
            foreach (var countryToCreate in countriesToCreate)
            {
                TestCountriesCreated.Add(CountriesClient.CreateCountry(countryToCreate).GetAwaiter().GetResult());
            }
        }

        private static void Cleanup(IEnumerable<Country> testCountriesToCleanup)
        {
            foreach (var testCountryToCleanup in testCountriesToCleanup)
            {
                var uri = new Uri(Path.Combine(ConfigurationManager.AppSettings[Constants.ApiEndpointKey], Routes.CountryV1BaseRoute, testCountryToCleanup.Id.ToString(), "force"));

                using (var client = new HttpClient())
                {
                    using (var response = client.DeleteAsync(uri).GetAwaiter().GetResult())
                    {
                        if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.Content.ToString());
                    }
                }
            }
        }
        #endregion
    }
}
