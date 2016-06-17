using System;
using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.SpecFlow
{
    public static class CountryAssertExtentions
    {
        public static void AreEqual(Country expected, Country actual)
        {
            CorePropertiesAreEqual(expected, actual);
            Assert.AreEqual(expected.CreatedByUserId, actual.CreatedByUserId);
            Assert.AreEqual(expected.CreatedOn, actual.CreatedOn);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.LastUpdatedByUserId, actual.LastUpdatedByUserId);
            Assert.AreEqual(expected.LastUpdatedOn, actual.LastUpdatedOn);
        }

        public static void CorePropertiesAreEqual(Country expected, Country actual)
        {
            Assert.AreEqual(expected.Alpha2Code, actual.Alpha2Code);
            Assert.AreEqual(expected.Alpha3Code, actual.Alpha3Code);
            Assert.AreEqual(expected.EffectiveEndDate, actual.EffectiveEndDate);
            Assert.AreEqual(expected.EffectiveStartDate, actual.EffectiveStartDate);
            Assert.AreEqual(expected.FullName, actual.FullName);
            Assert.AreEqual(expected.Iso3166Code, actual.Iso3166Code);
            Assert.AreEqual(expected.PhoneNumberRegex, actual.PhoneNumberRegex);
            Assert.AreEqual(expected.PostalCodeRegex, actual.PostalCodeRegex);
            Assert.AreEqual(expected.ShortName, actual.ShortName);
        }

        public static void CreatedCountryAuditInformationIsCorrect(Country createdCountry)
        {
            //TODO: figure out how to add a user for integration tests
            Assert.AreEqual(-365, createdCountry.CreatedByUserId);
            Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < createdCountry.CreatedOn.UtcDateTime && createdCountry.CreatedOn.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));

            Assert.IsNull(createdCountry.LastUpdatedByUserId);
            Assert.IsNull(createdCountry.LastUpdatedOn);
        }
    }
}
