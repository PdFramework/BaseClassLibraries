﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace IntegrationTests.SpecFlow
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class CountriesTestsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CountriesTests.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CountriesTests", "\tIn order for addresses to have associated countries\r\n\tAs an EHIM user\r\n\tI want t" +
                    "o be able to manage the country data in our system", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "CountriesTests")))
            {
                IntegrationTests.SpecFlow.CountriesTestsFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Create a country")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CountriesTests")]
        public virtual void CreateACountry()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create a country", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alpha2Code",
                        "Alpha3Code",
                        "EffectiveEndDate",
                        "EffectiveStartDate",
                        "FullName",
                        "Iso3166Code",
                        "PhoneNumberRegex",
                        "PostalCodeRegex",
                        "ShortName"});
            table1.AddRow(new string[] {
                        "US",
                        "USA",
                        "",
                        "7/4/1776",
                        "the United States of America",
                        "840",
                        "^1?[. -]?\\(?\\d{3}\\)?[. -]? *\\d{3}[. -]? *[. -]?\\d{4}$",
                        "^\\d{5}[ -]?\\d{4}$",
                        "United States Of America"});
#line 7
 testRunner.Given("the following countries", ((string)(null)), table1, "Given ");
#line 10
 testRunner.When("post is invoked on the countries api", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 11
 testRunner.Then("the country should be returned with an id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Get a country")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CountriesTests")]
        public virtual void GetACountry()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get a country", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alpha2Code",
                        "Alpha3Code",
                        "EffectiveEndDate",
                        "EffectiveStartDate",
                        "FullName",
                        "Iso3166Code",
                        "PhoneNumberRegex",
                        "PostalCodeRegex",
                        "ShortName"});
            table2.AddRow(new string[] {
                        "US",
                        "USA",
                        "",
                        "7/4/1776",
                        "the United States of America",
                        "840",
                        "^1?[. -]?\\(?\\d{3}\\)?[. -]? *\\d{3}[. -]? *[. -]?\\d{4}$",
                        "^\\d{5}[ -]?\\d{4}$",
                        "United States Of America"});
#line 14
 testRunner.Given("the following countries exist in the data store", ((string)(null)), table2, "Given ");
#line 17
 testRunner.When("get is invoked on the countries api with the country\'s id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("the country should be returned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Update a country")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CountriesTests")]
        public virtual void UpdateACountry()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update a country", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alpha2Code",
                        "Alpha3Code",
                        "EffectiveEndDate",
                        "EffectiveStartDate",
                        "FullName",
                        "Iso3166Code",
                        "PhoneNumberRegex",
                        "PostalCodeRegex",
                        "ShortName"});
            table3.AddRow(new string[] {
                        "SU",
                        "ASU",
                        "12/31/3000",
                        "4/7/1767",
                        "the United Staets of America",
                        "804",
                        "^1?[. -]?\\?[. -]? *\\d{3}[. -]? *[. -]?\\d{4}$",
                        "^\\d{5}[ -]?$",
                        "United Staets Of America"});
#line 21
 testRunner.Given("the following countries exist in the data store", ((string)(null)), table3, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alpha2Code",
                        "Alpha3Code",
                        "EffectiveEndDate",
                        "EffectiveStartDate",
                        "FullName",
                        "Iso3166Code",
                        "PhoneNumberRegex",
                        "PostalCodeRegex",
                        "ShortName"});
            table4.AddRow(new string[] {
                        "US",
                        "USA",
                        "",
                        "7/4/1776",
                        "the United States of America",
                        "840",
                        "^1?[. -]?\\(?\\d{3}\\)?[. -]? *\\d{3}[. -]? *[. -]?\\d{4}$",
                        "^\\d{5}[ -]?\\d{4}$",
                        "United States Of America"});
#line 24
 testRunner.When("update is invoked on the countries api with the following values", ((string)(null)), table4, "When ");
#line 27
 testRunner.Then("the country should be updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Soft delete a country")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CountriesTests")]
        public virtual void SoftDeleteACountry()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Soft delete a country", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alpha2Code",
                        "Alpha3Code",
                        "EffectiveEndDate",
                        "EffectiveStartDate",
                        "FullName",
                        "Iso3166Code",
                        "PhoneNumberRegex",
                        "PostalCodeRegex",
                        "ShortName"});
            table5.AddRow(new string[] {
                        "US",
                        "USA",
                        "",
                        "7/4/1776",
                        "the United States of America",
                        "840",
                        "^1?[. -]?\\(?\\d{3}\\)?[. -]? *\\d{3}[. -]? *[. -]?\\d{4}$",
                        "^\\d{5}[ -]?\\d{4}$",
                        "United States Of America"});
#line 30
 testRunner.Given("the following countries exist in the data store", ((string)(null)), table5, "Given ");
#line 33
 testRunner.When("delete is invoked on the countries api with the country\'s id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 34
 testRunner.Then("the country should exist in the data store", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 35
 testRunner.And("the country\'s effective end date should be updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Delete a country")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "CountriesTests")]
        public virtual void DeleteACountry()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete a country", ((string[])(null)));
#line 37
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Alpha2Code",
                        "Alpha3Code",
                        "EffectiveEndDate",
                        "EffectiveStartDate",
                        "FullName",
                        "Iso3166Code",
                        "PhoneNumberRegex",
                        "PostalCodeRegex",
                        "ShortName"});
            table6.AddRow(new string[] {
                        "US",
                        "USA",
                        "",
                        "7/4/1776",
                        "the United States of America",
                        "840",
                        "^1?[. -]?\\(?\\d{3}\\)?[. -]? *\\d{3}[. -]? *[. -]?\\d{4}$",
                        "^\\d{5}[ -]?\\d{4}$",
                        "United States Of America"});
#line 38
 testRunner.Given("the following countries exist in the data store", ((string)(null)), table6, "Given ");
#line 41
 testRunner.When("force delete is invoked on the countries api with the country\'s id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 42
 testRunner.Then("the country should not exist in the data store", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
