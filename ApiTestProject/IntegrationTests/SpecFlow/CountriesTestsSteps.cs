namespace IntegrationTests.SpecFlow
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Contracts;
    using PeinearyDevelopment.Framework.BaseClassLibraries.Testing.SpecFlow;

    using ApiClients;
    using Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;
    using System.Configuration;
    using System.IO;

    [Binding, Scope(Tag = "Countries")]
    public class CountriesTestsSteps
    {
        #region Constructor & Properties
        private readonly ScenarioDateRangeValidItemsContext<Country, int> _scenarioContext;

        public CountriesTestsSteps(ScenarioDateRangeValidItemsContext<Country, int> scenarioContext)
        {
            scenarioContext.ItemsClient = new CountriesApiClient();
            scenarioContext.ItemsGenerator = new CountryGenerator();
            scenarioContext.ForceDeleteItemPathFormat = Path.Combine(ConfigurationManager.AppSettings[Constants.ApiEndpointKey], Routes.CountryV1BaseRoute, "{0}", "force");
            _scenarioContext = scenarioContext;
        }
        #endregion

        #region Given
        [Given(@"a valid country")]
        public void Given_AValidCountry()
        {
            _scenarioContext.GenerateValidItem();
        }

        [Given(@"a valid country exists in the system")]
        public void Given_AValidCountryExistsInTheSystem()
        {
            Given_AValidCountry();
            When_PostIsInvokedOnTheCountriesApi();
        }
        #endregion

        #region When
        [When(@"POST is invoked on the countries' api")]
        public void When_PostIsInvokedOnTheCountriesApi()
        {
            _scenarioContext.CreateItem();
        }

        [When(@"the country is updated")]
        public void When_SomeOfTheCountriesPropertiesAreUpdated()
        {
            _scenarioContext.GenerateValidUpdatedItem();
        }

        [When(@"GET is invoked on the countries' api with the country's id")]
        public void When_GetIsInvokedOnTheCountriesApiWithTheCountrysId()
        {
            _scenarioContext.GetItem(ItemContextState.Created);
        }

        [When(@"PUT is invoked on the countries' api")]
        public void When_PutIsInvokedOnTheCountriesApi()
        {
            _scenarioContext.UpdateItem();
        }

        [When(@"DELETE is invoked on the countries' api with the country's id")]
        public void When_DeleteIsInvokedOnTheCountriesApiWithTheCountrysId()
        {
            _scenarioContext.DeleteItem();
        }

        [When(@"force DELETE is invoked on the countries' api with the country's id")]
        public void When_ForceDeleteIsInvokedOnTheCountriesApiWithTheCountrysId()
        {
            _scenarioContext.ForceDeleteItem();
        }
        #endregion

        #region Then
        [Then(@"the country should be returned from the POST")]
        public void Then_TheCountryShouldBeReturnedFromThePost()
        {
            var expected = _scenarioContext.GetStatefulItem(ItemContextState.CurrentTestState);
            var actual = _scenarioContext.GetStatefulItem(ItemContextState.Created);
            CountryAssertExtentions.CorePropertiesAreEqual(expected, actual);
        }

        [Then(@"it should contain a service generated id")]
        public void Then_ItShouldContainAServiceGeneratedId()
        {
            Assert.AreNotEqual(_scenarioContext.GetStatefulItem(ItemContextState.CurrentTestState).Id, _scenarioContext.GetStatefulItem(ItemContextState.Created).Id);
        }

        [Then(@"it should have the correct created audit information")]
        public void Then_ItShouldHaveTheCorrectCreatedAuditInformation()
        {
            var createdCountry = _scenarioContext.GetStatefulItem(ItemContextState.Created);
            CountryAssertExtentions.CreatedCountryAuditInformationIsCorrect(createdCountry);
        }

        [Then(@"the (.*) country should be stored in the system")]
        public void Then_TheCountryShouldBeStoredInTheSystem(ItemContextState objectState)
        {
            CompareCountryInStateWithStoredCountry(objectState);
        }

        [Then(@"the country should be returned")]
        public void Then_TheCountryShouldBeReturned()
        {
            CompareCountryInStateWithStoredCountry(ItemContextState.Created);
        }

        [Then(@"the country should be returned from the PUT")]
        public void Then_TheCountryShouldBeReturnedFromThePut()
        {
            var expected = _scenarioContext.GetStatefulItem(ItemContextState.CurrentTestState);
            var actual = _scenarioContext.GetStatefulItem(ItemContextState.Updated);
            CountryAssertExtentions.CorePropertiesAreEqual(expected, actual);
        }

        [Then(@"the (.*) country should have the correct updated audit information")]
        public void Then_TheCountryShouldHaveTheCorrectUpdatedAuditInformation(ItemContextState objectState)
        {
            var createdCountry = _scenarioContext.GetStatefulItem(ItemContextState.Created);
            var lastUpdatedCountry = _scenarioContext.GetStatefulItem(objectState);
            CountryAssertExtentions.UpdatedCountryAuditInformationIsCorrect(createdCountry, lastUpdatedCountry);
        }

        [Then(@"the country's effective end date should be updated")]
        public void Then_TheCountrysEffectiveEndDateShouldBeUpdated()
        {
            _scenarioContext.GetItem(ItemContextState.Deleted);
            CountryAssertExtentions.DateTimeOffsetInRecentlyUpdatedRange(_scenarioContext.GetStatefulItem(ItemContextState.Retrieved).EffectiveEndDate);
        }

        [Then(@"the country should not be stored in the system")]
        public void Then_TheCountryShouldNotBeStoredInTheSystem()
        {
            try
            {
                _scenarioContext.GetItem(ItemContextState.Deleted);
                Assert.Fail("Country wasn't deleted.");
            }
            catch (ApiInvokerException)
            {
            }
        }
        #endregion

        #region Cleanup
        [AfterScenario]
        public void CleanupAfterScenario()
        {
            _scenarioContext.Cleanup();
        }
        #endregion

        #region Helper Methods
        private void CompareCountryInStateWithStoredCountry(ItemContextState itemContextState)
        {
            _scenarioContext.GetItem(ItemContextState.Created);
            var expected = _scenarioContext.GetStatefulItem(itemContextState);
            var actual = _scenarioContext.GetStatefulItem(ItemContextState.Retrieved);
            CountryAssertExtentions.AreEqual(expected, actual);
        }
        #endregion
    }
}
