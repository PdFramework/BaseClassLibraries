namespace DataAccessContracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesDal
    {
        Task<CountryDto> Create(CountryDto country);
        Task<CountryDto> Read(int countryId);
        Task<CountryDto> Update(CountryDto country);
        Task Delete(int countryId);
        Task<IEnumerable<CountryDto>> GetMatchingCountries(CountryDto country);
    }
}
