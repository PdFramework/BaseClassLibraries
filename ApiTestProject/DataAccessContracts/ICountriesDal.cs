namespace DataAccessContracts
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess.Contracts;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesDal : IDalBase<CountryDto, int>
    {
        Task<IEnumerable<CountryDto>> GetMatchingCountries(CountryDto country);
    }
}
