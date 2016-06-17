using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using ApiClients;
using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace IntegrationTests.SpecFlow
{
    [Binding]
    public class RandomDataCountriesTestsSteps
    {
        private List<Country> CountriesCreated { get; set; }
        //private List<Country> CountriesUpdated { get; set; }
        private readonly ICountriesClient _countriesClient;
        private readonly CountryGenerator _countryGenerator;

        //private Country TestCountry { get; set; }
        //private Country CountryReturnedFromAction { get; set; }
        private Dictionary<ObjectState, Country> TestContext { get; set; }

        public RandomDataCountriesTestsSteps()
        {
            _countriesClient = new CountriesApiClient();
            _countryGenerator = new CountryGenerator();
        }

        [BeforeScenario]
        public void SetupBeforeScenario()
        {
            CountriesCreated = new List<Country>();
            TestContext = new Dictionary<ObjectState, Country>();
            //CountriesUpdated = new List<Country>();
        }

        [Given(@"a valid country")]
        public void Given_AValidCountry()
        {
            TestContext.Add(ObjectState.CurrentTestState, _countryGenerator.GenerateValidCountry());
            //TestCountry = _countryGenerator.GenerateValidCountry();
        }

        [Given(@"a valid country exists in the system")]
        public void Given_AValidCountryExistsInTheSystem()
        {
            Given_AValidCountry();
            When_PostIsInvokedOnTheCountriesApi();
        }

        [When(@"POST is invoked on the countries api")]
        public void When_PostIsInvokedOnTheCountriesApi()
        {
            CreateCountry(TestContext[ObjectState.CurrentTestState]);
            //CreateCountry(TestCountry);
        }

        private void CreateCountry(Country countryToCreate)
        {
            TestContext.Add(ObjectState.LastCreated, _countriesClient.CreateCountry(countryToCreate).GetAwaiter().GetResult());
            CountriesCreated.Add(TestContext[ObjectState.LastCreated]);
            //CountryReturnedFromAction = _countriesClient.CreateCountry(countryToCreate).GetAwaiter().GetResult();
            //CountriesCreated.Add(CountryReturnedFromAction);
        }

        //[When(@"some of the countries properties are updated")]
        //public void When_SomeOfTheCountriesPropertiesAreUpdated()
        //{
        //    var countryIdToUpdate = CountriesCreated.First().Id;
        //    TestCountry = _countryGenerator.GenerateUpdatedValidCountry(countryIdToUpdate);
        //}



        [When(@"GET is invoked on the countries api with the country's id")]
        public void When_GetIsInvokdeOnTheCountriesApiWithTheCountrysId()
        {
            TestContext.Add(ObjectState.LastRetrieved, _countriesClient.GetCountry(CountriesCreated.First().Id).GetAwaiter().GetResult());
            //TestCountry = _countriesClient.GetCountry(CountriesCreated.First().Id).GetAwaiter().GetResult();
        }

        //[When(@"PUT is invoked on the countries api")]
        //public void When_PutIsInvokedOnTheCountriesApi()
        //{
        //    CountriesUpdated.Add(_countriesClient.UpdateCountry(TestCountry).GetAwaiter().GetResult());
        //}

        //[Then(@"the( updated)? country should be stored in the system")]
        //public void Then_TheCountryShouldBeStoredInTheSystem()
        //{

        //}

        [Then(@"the country should be returned from the POST")]
        public void Then_TheCountryShouldBeReturnedFromThePost()
        {
            CountryAssertExtentions.CorePropertiesAreEqual(TestContext[ObjectState.CurrentTestState], TestContext[ObjectState.LastCreated]);
            //CountryAssertExtentions.CorePropertiesAreEqual(TestCountry, CountryReturnedFromAction);
        }

        [Then(@"it should contain a service generated id")]
        public void Then_ItShouldContainAServiceGeneratedId()
        {
            Assert.AreNotEqual(TestContext[ObjectState.CurrentTestState].Id, TestContext[ObjectState.LastCreated].Id);
            //Assert.AreNotEqual(TestCountry.Id, CountryReturnedFromAction.Id);
        }

        [Then(@"it should have the correct created audit information")]
        public void Then_ItShouldHaveTheCorrectCreatedAuditInformation()
        {
            CountryAssertExtentions.CreatedCountryAuditInformationIsCorrect(TestContext[ObjectState.LastCreated]);
            //CountryAssertExtentions.CreatedCountryAuditInformationIsCorrect(CountryReturnedFromAction);
        }

        [Then(@"the country should be stored in the system")]
        [Then(@"all of the country information should be returned")]
        public void Then_TheCountryShouldBeStoredInTheSystem()
        {
            var storedCountry = _countriesClient.GetCountry(TestContext[ObjectState.LastCreated].Id).GetAwaiter().GetResult();
            CountryAssertExtentions.AreEqual(TestContext[ObjectState.LastCreated], storedCountry);
            //var storedCountry = _countriesClient.GetCountry(CountryReturnedFromAction.Id).GetAwaiter().GetResult();
            //CountryAssertExtentions.AreEqual(CountryReturnedFromAction, storedCountry);
        }

        ////[Then(@"the country should be returned")]
        ////public void Then_TheCountryShouldBeReturned()
        ////{
        ////    var createdCountry = CountriesCreated.First();

        ////    Assert.AreEqual(createdCountry.Alpha2Code, TestCountry.Alpha2Code);
        ////    Assert.AreEqual(createdCountry.Alpha3Code, TestCountry.Alpha3Code);
        ////    Assert.AreEqual(createdCountry.CreatedByUserId, TestCountry.CreatedByUserId);
        ////    Assert.AreEqual(createdCountry.EffectiveEndDate, TestCountry.EffectiveEndDate);
        ////    Assert.AreEqual(createdCountry.EffectiveStartDate, TestCountry.EffectiveStartDate);
        ////    Assert.AreEqual(createdCountry.FullName, TestCountry.FullName);
        ////    Assert.AreEqual(createdCountry.Iso3166Code, TestCountry.Iso3166Code);
        ////    Assert.AreEqual(createdCountry.PhoneNumberRegex, TestCountry.PhoneNumberRegex);
        ////    Assert.AreEqual(createdCountry.PostalCodeRegex, TestCountry.PostalCodeRegex);
        ////    Assert.AreEqual(createdCountry.ShortName, TestCountry.ShortName);
        ////    Assert.AreEqual(createdCountry.Id, TestCountry.Id);
        ////    Assert.AreEqual(createdCountry.CreatedOn, TestCountry.CreatedOn.UtcDateTime);
        ////    Assert.AreEqual(createdCountry.LastUpdatedByUserId, TestCountry.LastUpdatedByUserId);
        ////    Assert.AreEqual(createdCountry.LastUpdatedOn, TestCountry.LastUpdatedOn);
        ////}

        //[Then(@"the country should be updated")]
        //public void Then_TheCountryShouldBeUpdated()
        //{
        //    //var createdCountry = CountriesCreated.First(c => c.Id == TestCountry.Id);
        //    //CountryRecievedFromApi = CountriesClient.GetCountry(createdCountry.Id).GetAwaiter().GetResult();

        //    //Assert.AreEqual(CountryValuesToUpdate.Alpha2Code, CountryRecievedFromApi.Alpha2Code);
        //    //Assert.AreEqual(CountryValuesToUpdate.Alpha3Code, CountryRecievedFromApi.Alpha3Code);
        //    //Assert.AreEqual(createdCountry.CreatedByUserId, CountryRecievedFromApi.CreatedByUserId);
        //    //Assert.AreEqual(CountryValuesToUpdate.EffectiveEndDate, CountryRecievedFromApi.EffectiveEndDate);
        //    //Assert.AreEqual(CountryValuesToUpdate.EffectiveStartDate, CountryRecievedFromApi.EffectiveStartDate);
        //    //Assert.AreEqual(CountryValuesToUpdate.FullName, CountryRecievedFromApi.FullName);
        //    //Assert.AreEqual(CountryValuesToUpdate.Iso3166Code, CountryRecievedFromApi.Iso3166Code);
        //    //Assert.AreEqual(CountryValuesToUpdate.PhoneNumberRegex, CountryRecievedFromApi.PhoneNumberRegex);
        //    //Assert.AreEqual(CountryValuesToUpdate.PostalCodeRegex, CountryRecievedFromApi.PostalCodeRegex);
        //    //Assert.AreEqual(CountryValuesToUpdate.ShortName, CountryRecievedFromApi.ShortName);
        //    //Assert.AreEqual(createdCountry.Id, CountryRecievedFromApi.Id);
        //    //Assert.AreEqual(createdCountry.CreatedOn, CountryRecievedFromApi.CreatedOn.UtcDateTime);
        //    //Assert.AreEqual(-365, CountryRecievedFromApi.LastUpdatedByUserId);
        //    //Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < CountryRecievedFromApi.LastUpdatedOn.Value.UtcDateTime && CountryRecievedFromApi.LastUpdatedOn.Value.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));
        //}

        #region Cleanup
        [AfterScenario]
        public void CleanupAfterScenario()
        {
            Cleanup(CountriesCreated);
        }
        #endregion

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
    }
}
