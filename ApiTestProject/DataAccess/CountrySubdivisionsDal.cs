namespace DataAccess
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccessContracts;
    using PeinearyDevelopment.Framework.BaseClassLibraries.Data.Entity;

    public class CountrySubdivisionsDal : DalBase<CountrySubdivisionDto, int>, ICountrySubdivisionsDal
    {
        public CountrySubdivisionsDal(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CountrySubdivisionDto>> GetMatchingCountrySubdivisions(CountrySubdivisionDto countrySubdivision)
        {
            using (var context = new CountriesDbContext())
            {
                return await context.CountrySubdivisions.Where(countrySubdivisionDto => countrySubdivisionDto.Id != countrySubdivision.Id && countrySubdivisionDto.CountryId == countrySubdivision.CountryId && countrySubdivisionDto.Name == countrySubdivision.Name).ToArrayAsync();
            }
        }
    }
}
