namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccessContracts;

    public class CountriesDal : ICountriesDal
    {
        public async Task<CountryDto> Create(CountryDto country)
        {
            using (var context = new CountriesDbContext())
            {
                var createdCountry = context.Countries.Add(country);

                await context.SaveChangesAsync();
                return createdCountry;
            }
        }

        public async Task<CountryDto> Read(int countryId)
        {
            using (var context = new CountriesDbContext())
            {
                var dbCountry = await context.Countries.FirstOrDefaultAsync(country => country.Id == countryId);
                if (dbCountry == null) throw new ObjectNotFoundException();

                return dbCountry;
            }
        }

        public async Task<CountryDto> Update(CountryDto country)
        {
            using (var context = new CountriesDbContext())
            {
                var dbCountry = await context.Countries.FirstOrDefaultAsync(countryDto => countryDto.Id == country.Id);
                if (country == null) throw new ObjectNotFoundException();

                dbCountry.LastUpdatedByUserId = country.LastUpdatedByUserId;
                dbCountry.LastUpdatedOn = DateTimeOffset.UtcNow;
                dbCountry.Alpha2Code = country.Alpha2Code;
                dbCountry.Alpha3Code = country.Alpha3Code;
                dbCountry.FullName = country.FullName;
                dbCountry.EffectiveEndDate = country.EffectiveEndDate;
                dbCountry.EffectiveStartDate = country.EffectiveStartDate;
                dbCountry.Iso3166Code = country.Iso3166Code;
                dbCountry.ShortName = country.ShortName;
                dbCountry.PhoneNumberRegex = country.PhoneNumberRegex;
                dbCountry.PostalCodeRegex = country.PostalCodeRegex;

                await context.SaveChangesAsync();
                return dbCountry;
            }
        }

        public async Task Delete(int countryId)
        {
            using (var context = new CountriesDbContext())
            {
                var dbCountry = await context.Countries.FirstOrDefaultAsync(country => country.Id == countryId);
                if (dbCountry == null) throw new ObjectNotFoundException();

                context.Countries.Remove(dbCountry);

                await context.SaveChangesAsync();
            }
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
