namespace ApiClients
{
    using Contracts;

    using System.Threading.Tasks;
    using PeinearyDevelopment.Framework.BaseClassLibraries.Apis.Clients;

    public class CountriesApiClient : ICountriesClient
    {
        public async Task<Country> GetCountry(int id)
        {
            return await ApiInvoker.Get<Country, int>(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, id);
        }

        public async Task<Country> CreateCountry(Country country)
        {
            return await ApiInvoker.Create(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, country);
        }

        public async Task<Country> UpdateCountry(Country country)
        {
            return await ApiInvoker.Update(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, country);
        }

        public async Task DeleteCountry(int id)
        {
            await ApiInvoker.Delete(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, id);
        }
    }
}
