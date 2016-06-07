namespace Contracts
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Contracts;

    public class CountrySubdivision : DateRangeEffectiveContractBase<int>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public CountrySubdivisionType Type { get; set; }
        public string Iso31662Code { get; set; }
        public string PostalCodeRegex { get; set; }
        public string PhoneNumberRegex { get; set; }
        public bool IsPrinicpalCountrySubdivision { get; set; }
        public int CountryId { get; set; }
        public int? ParentCountrySubdivisionId { get; set; }
    }
}
