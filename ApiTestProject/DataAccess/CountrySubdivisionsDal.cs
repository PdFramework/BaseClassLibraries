namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccessContracts;

    public class CountrySubdivisionsDal : ICountrySubdivisionsDal
    {
        public async Task<CountrySubdivisionDto> Create(CountrySubdivisionDto countrySubdivision)
        {
            using (var context = new CountriesDbContext())
            {
                countrySubdivision.CreatedOn = DateTimeOffset.UtcNow;
                countrySubdivision.LastUpdatedByUserId = null;
                countrySubdivision.LastUpdatedOn = null;
                var createdCountrySubdivision = context.CountrySubdivisions.Add(countrySubdivision);

                await context.SaveChangesAsync();
                return createdCountrySubdivision;
            }
        }

        public async Task<CountrySubdivisionDto> Read(int countrySubdivisionId)
        {
            using (var context = new CountriesDbContext())
            {
                var dbCountrySubdivision = await context.CountrySubdivisions.FirstOrDefaultAsync(countrySubdivision => countrySubdivision.Id == countrySubdivisionId);
                if(dbCountrySubdivision == null) throw new ObjectNotFoundException();

                return dbCountrySubdivision;
            }
        }

        public async Task<CountrySubdivisionDto> Update(CountrySubdivisionDto countrySubdivision)
        {
            using (var context = new CountriesDbContext())
            {
                var dbCountrySubdivision = await context.CountrySubdivisions.FirstOrDefaultAsync(subdivision => subdivision.Id == countrySubdivision.Id);
                if (countrySubdivision == null) throw new ObjectNotFoundException();

                dbCountrySubdivision.LastUpdatedByUserId = countrySubdivision.LastUpdatedByUserId;
                dbCountrySubdivision.LastUpdatedOn = DateTimeOffset.UtcNow;
                dbCountrySubdivision.Abbreviation = countrySubdivision.Abbreviation;
                dbCountrySubdivision.CountryId = countrySubdivision.CountryId;
                dbCountrySubdivision.ParentCountrySubdivisionId = countrySubdivision.ParentCountrySubdivisionId;
                dbCountrySubdivision.EffectiveEndDate = countrySubdivision.EffectiveEndDate;
                dbCountrySubdivision.EffectiveStartDate = countrySubdivision.EffectiveStartDate;
                dbCountrySubdivision.IsPrinicpalCountrySubdivision = countrySubdivision.IsPrinicpalCountrySubdivision;
                dbCountrySubdivision.Iso31662Code = countrySubdivision.Iso31662Code;
                dbCountrySubdivision.Name = countrySubdivision.Name;
                dbCountrySubdivision.PhoneNumberRegex = countrySubdivision.PhoneNumberRegex;
                dbCountrySubdivision.PostalCodeRegex = countrySubdivision.PostalCodeRegex;

                await context.SaveChangesAsync();
                return dbCountrySubdivision;
            }
        }

        public async Task Delete(int countrySubdivisionId)
        {
            using (var context = new CountriesDbContext())
            {
                var countrySubdivision = await context.CountrySubdivisions.FirstOrDefaultAsync(subdivision => subdivision.Id == countrySubdivisionId);
                if (countrySubdivision == null) throw new ObjectNotFoundException();

                context.CountrySubdivisions.Remove(countrySubdivision);

                await context.SaveChangesAsync();
            }
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
