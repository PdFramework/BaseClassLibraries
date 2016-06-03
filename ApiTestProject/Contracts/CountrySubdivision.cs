namespace Contracts
{
    using System;

    public class CountrySubdivision
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public CountrySubdivisionType Type { get; set; }
        public string Iso31662Code { get; set; }
        public string PostalCodeRegex { get; set; }
        public string PhoneNumberRegex { get; set; }
        public bool IsPrinicpalCountrySubdivision { get; set; }
        public DateTimeOffset EffectiveStartDate { get; set; }
        public DateTimeOffset? EffectiveEndDate { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int? LastUpdatedByUserId { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
        public int CountryId { get; set; }
        public int? ParentCountrySubdivisionId { get; set; }
    }
}
