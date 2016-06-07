namespace DataAccessContracts
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess.Contracts;

    public class CountrySubdivisionDto : DateRangeEffectiveDtoBase<int>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public CountrySubdivisionTypeDto Type { get; set; }
        public string Iso31662Code { get; set; }
        public string PostalCodeRegex { get; set; }
        public string PhoneNumberRegex { get; set; }
        public bool IsPrinicpalCountrySubdivision { get; set; }

        public int CountryId { get; set; }
        public virtual CountryDto Country { get; set; }

        public int? ParentCountrySubdivisionId { get; set; }
        public virtual CountrySubdivisionDto ParentCountrySubdivision { get; set; }
    }
}
