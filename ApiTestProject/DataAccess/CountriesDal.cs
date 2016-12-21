namespace DataAccess
{
    using DataAccessContracts;

    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using PeinearyDevelopment.Framework.BaseClassLibraries.Data.Entity;

    public class CountriesDal : DalBase<CountryDto, int>, ICountriesDal
    {

        public CountriesDal(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<CountryDto>> GetMatchingCountries(CountryDto country)
        {
            using (var context = new CountriesDbContext())
            {
                return await context.Countries.Where(countryDto => countryDto.FullName == country.FullName && countryDto.Alpha2Code == country.Alpha2Code && countryDto.Id != country.Id).ToArrayAsync();
            }
        }
    }
}
