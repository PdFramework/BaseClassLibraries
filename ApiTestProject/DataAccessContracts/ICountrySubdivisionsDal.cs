namespace DataAccessContracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountrySubdivisionsDal
    {
        Task<CountrySubdivisionDto> Create(CountrySubdivisionDto countrySubdivision);
        Task<CountrySubdivisionDto> Read(int countrySubdivisionId);
        Task<CountrySubdivisionDto> Update(CountrySubdivisionDto countrySubdivision);
        Task Delete(int countrySubdivisionId);
        Task<IEnumerable<CountrySubdivisionDto>> GetMatchingCountrySubdivisions(CountrySubdivisionDto countrySubdivision);
    }
}
