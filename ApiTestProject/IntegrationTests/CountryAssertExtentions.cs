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
            DateTimeOffsetInRecentlyUpdatedRange(createdCountry.CreatedOn);

            Assert.IsNull(createdCountry.LastUpdatedByUserId);
            Assert.IsNull(createdCountry.LastUpdatedOn);
        }

        public static void UpdatedCountryAuditInformationIsCorrect(Country createdCountry, Country lastUpdatedCountry)
        {
            //TODO: figure out how to add a user for integration tests
            Assert.AreEqual(createdCountry.CreatedByUserId, createdCountry.CreatedByUserId);
            Assert.AreEqual(createdCountry.CreatedOn, createdCountry.CreatedOn);

            Assert.AreEqual(-365, lastUpdatedCountry.LastUpdatedByUserId);
            DateTimeOffsetInRecentlyUpdatedRange(lastUpdatedCountry.LastUpdatedOn);
        }

        public static void SoftDeletedCountryEffectiveEndDateIsCorrect(Country softDeletedCountry)
        {
            DateTimeOffsetInRecentlyUpdatedRange(softDeletedCountry.EffectiveEndDate);
        }

        public static void DateTimeOffsetInRecentlyUpdatedRange(DateTimeOffset dateTimeOffset)
        {
            Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < dateTimeOffset.UtcDateTime && dateTimeOffset.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));
        }

        public static void DateTimeOffsetInRecentlyUpdatedRange(DateTimeOffset? dateTimeOffset)
        {
            Assert.IsTrue(DateTimeOffset.UtcNow.AddSeconds(-1) < dateTimeOffset.Value.UtcDateTime && dateTimeOffset.Value.UtcDateTime < DateTimeOffset.UtcNow.AddSeconds(1));
        }
    }
}
