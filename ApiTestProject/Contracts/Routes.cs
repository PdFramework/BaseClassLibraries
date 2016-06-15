namespace Contracts
{
    public static class Routes
    {
        public const string CountryControllerName = "Countries";
        public const string CountryV1BaseRoute = "v1/" + CountryControllerName;
        public const string GetCountryNamedRouteName = "GetCountry";

        public const string CountrySubdivisionControllerName = "CountrySubdivisions";
        public const string CountrySubdivisionV1BaseRoute = "v1/" + CountrySubdivisionControllerName;
        public const string GetCountrySubdivisionNamedRouteName = "GetCountrySubdivision";
    }
}
