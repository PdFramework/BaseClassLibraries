﻿namespace ApiClients
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.ApiClients;

    using Contracts;

    using System.Threading.Tasks;

    public class CountrySubdivisionsApiClient : ICountrySubdivisionsClient
    {
        public async Task<CountrySubdivision> GetCountrySubdivision(int id)
        {
            return await ApiInvoker.Get<CountrySubdivision, int>(Constants.ApiEndpointKey, Constants.CountrySubdivisionsApiBaseRoute, id);
        }

        public async Task<CountrySubdivision> CreateCountrySubdivision(CountrySubdivision countrySubdivision)
        {
            return await ApiInvoker.Create(Constants.ApiEndpointKey, Constants.CountrySubdivisionsApiBaseRoute, countrySubdivision);
        }

        public async Task<CountrySubdivision> UpdateCountrySubdivision(CountrySubdivision countrySubdivision)
        {
            return await ApiInvoker.Update(Constants.ApiEndpointKey, Constants.CountrySubdivisionsApiBaseRoute, countrySubdivision);
        }

        public async Task DeleteCountrySubdivision(int id)
        {
            await ApiInvoker.Delete(Constants.ApiEndpointKey, Constants.CountrySubdivisionsApiBaseRoute, id);
        }
    }
}
