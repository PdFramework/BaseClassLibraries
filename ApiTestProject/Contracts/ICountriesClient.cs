namespace Contracts
{
    using System.Threading.Tasks;

    public interface ICountriesClient
    {
        Task<Country> GetCountry(int id);
        Task<Country> CreateCountry(Country country);
        Task<Country> UpdateCountry(Country country);
        Task DeleteCountry(int id);
    }
}
