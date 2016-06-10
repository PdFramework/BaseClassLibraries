
namespace DataAccessContracts
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess.Contracts;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountrySubdivisionsDal : IDalBase<CountrySubdivisionDto, int>
    {
        Task<IEnumerable<CountrySubdivisionDto>> GetMatchingCountrySubdivisions(CountrySubdivisionDto countrySubdivision);
    }
}
