namespace IntegrationTests.Non_SpecFlow
{
    using System;
    using System.Text;

    public class TestData
    {
        public TestData()
        {
            NonServiceGeneratedCountryCreatedOn = DateTimeOffset.UtcNow.AddYears(-3);
            NonServiceGeneratedCountryLastUpdatedOn = DateTimeOffset.UtcNow.AddDays(-3);
            CountryCreatedById = -365;
            CountryCreatedByIdForUpdate = 3;
            CountryLastUpdatedById = -365;

            NonServiceGeneratedCountryId = -1;
            // TODO: update to create values dynamically('randomly') so multiple integration tests could be run simultaneously as well run against non-test database
            UsCountryName = RandomString(255);
            UsCountryAlpha2Code = "US";
            UsCountryAlpha3Code = "USA";
            UsPhoneNumberRegex = @"^1?[. -]?\(?\d{3}\)?[. -]? *\d{3}[. -]? *[. -]?\d{4}$";
            UsPostalCodeRegex = @"^\d{5}[ -]?\d{4}$";
            CaCountryName = "Canada";
            CaCountryAbbreviation = "CA";
            CaPhoneNumberRegex = @"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$";
            CaPostalCodeRegex = @"^[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVxXyY]\d[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVwWxXyYzZ]( )?\d[aAbBcCeEgGhHjJkKlLmMnNpPrRsStTvVwWxXyYzZ]\d$";
        }

        public DateTimeOffset NonServiceGeneratedCountryCreatedOn { get; }
        public DateTimeOffset NonServiceGeneratedCountryLastUpdatedOn { get; }

        public int CountryCreatedById { get; }
        public int CountryCreatedByIdForUpdate { get; }
        public int CountryLastUpdatedById { get; }

        public int NonServiceGeneratedCountryId { get; }
        public string UsCountryName { get; }
        public string UsCountryAlpha2Code { get; }
        public string UsCountryAlpha3Code { get; }
        public string UsPhoneNumberRegex { get; }
        public string UsPostalCodeRegex { get; }
        public string CaCountryName { get; }
        public string CaCountryAbbreviation { get; }
        public string CaPhoneNumberRegex { get; }
        public string CaPostalCodeRegex { get; }

        //http://stackoverflow.com/questions/1122483/1874522/random-string-generator-returning-same-string#answer-1122519
        private static readonly Random Random = new Random((int) DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65))));
            }

            return builder.ToString();
        }
    }
}
