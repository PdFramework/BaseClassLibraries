namespace ApiClients
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.ApiClients;

    using Contracts;

    using System.Threading.Tasks;

    public class CountriesApiClient : ICountriesClient
    {
        public async Task<Country> GetCountry(int id)
        {
            return await ApiInvoker.Get<Country, int>(Constants.ApiEndpointKey, Constants.CountriesApiBaseRoute, id);
        }

        public async Task<Country> CreateCountry(Country country)
        {
            return await ApiInvoker.Create(Constants.ApiEndpointKey, Constants.CountriesApiBaseRoute, country);
        }

        public async Task<Country> UpdateCountry(Country country)
        {
            return await ApiInvoker.Update(Constants.ApiEndpointKey, Constants.CountriesApiBaseRoute, country);
        }

        public async Task DeleteCountry(int id)
        {
            await ApiInvoker.Delete(Constants.ApiEndpointKey, Constants.CountriesApiBaseRoute, id);
        }
    }
}
