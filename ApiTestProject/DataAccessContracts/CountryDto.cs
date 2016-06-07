namespace DataAccessContracts
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess.Contracts;

    public class CountryDto : DateRangeEffectiveDtoBase<int>
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public int Iso3166Code { get; set; }
        public string PostalCodeRegex { get; set; }
        public string PhoneNumberRegex { get; set; }
    }
}
