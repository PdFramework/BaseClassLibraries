namespace ApiClients
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Clients.Apis;

    using Contracts;

    using System.Threading.Tasks;

    public class CountriesApiClient : ICountriesClient
    {
        public async Task<Country> Get(int id)
        {
            return await ApiInvoker.Get<Country, int>(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, id);
        }

        public async Task<Country> Create(Country contract)
        {
            return await ApiInvoker.Create(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, contract);
        }

        public async Task<Country> Update(Country contract)
        {
            return await ApiInvoker.Update(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, contract);
        }

        public async Task Delete(int id)
        {
            await ApiInvoker.Delete(Constants.ApiEndpointKey, Routes.CountryV1BaseRoute, id);
        }
    }
}
