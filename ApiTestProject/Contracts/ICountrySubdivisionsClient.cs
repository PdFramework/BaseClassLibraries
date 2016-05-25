namespace Contracts
{
    using System.Threading.Tasks;

    public interface ICountrySubdivisionsClient
    {
        Task<CountrySubdivision> GetCountrySubdivision(int id);
        Task<CountrySubdivision> CreateCountrySubdivision(CountrySubdivision countrySubdivision);
        Task<CountrySubdivision> UpdateCountrySubdivision(CountrySubdivision countrySubdivision);
        Task DeleteCountrySubdivision(int id);
    }
}
