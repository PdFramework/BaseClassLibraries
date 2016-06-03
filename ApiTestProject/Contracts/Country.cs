namespace Contracts
{
    using System;

    public class Country
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public int Iso3166Code { get; set; }
        public string PostalCodeRegex { get; set; }
        public string PhoneNumberRegex { get; set; }
        public DateTimeOffset EffectiveStartDate { get; set; }
        public DateTimeOffset? EffectiveEndDate { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int? LastUpdatedByUserId { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
    }
}
