namespace Contracts
{
    public static class ErrorMessages
    {
        public const string CountryNameMissing = "Country's name can't be blank.";
        public const string CountryAbbreviationMissing = "Country's abbreviation can't be blank.";
        public const string NonUniqueCountryName = "Country name needs to be unique.";
        public const string NonUniqueCountryAbbreviation = "Country abbreviation needs to be unique.";

        public const string InvalidCountryId = "Invalid country id.";
        public const string InvalidParentCountrySubdivisionId = "Invalid parent country subdivision id.";
        public const string MismatchedParentCountrySubdivisionId = "The parent country subdivision chosen is part of a different country than the country chosen for this subdivision.";
        public const string CountrySubdivisionNameMissing = "Country subdivision's name can't be blank.";
        public const string NonUniqueCountrySubdivisionName = "Country subdivision name needs to be unique.";
        public const string NonUniqueCountrySubdivisionAbbreviation = "Country subdivision abbreviation needs to be unique.";
    }
}
