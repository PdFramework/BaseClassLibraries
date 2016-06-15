namespace Apis.ContractValidators
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using Contracts;
    using DataAccessContracts;

    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CountryValidator : IContractValidator<Country>
    {
        private ICountriesDal CountriesDal { get; }
        private IMapper Mapper { get; }

        public CountryValidator(ICountriesDal countriesDal, IMapper mapper)
        {
            CountriesDal = countriesDal;
            Mapper = mapper;
        }

        public async Task<IEnumerable<string>> ValidateContract(Country country)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(country.FullName)) errorMessages.Add(ErrorMessages.CountryNameMissing);
            if (string.IsNullOrWhiteSpace(country.Alpha2Code)) errorMessages.Add(ErrorMessages.CountryAbbreviationMissing);

            var matchingCountries = await CountriesDal.GetMatchingCountries(Mapper.Map<CountryDto>(country));
            if (matchingCountries.Any())
            {
                if (!string.IsNullOrWhiteSpace(country.FullName) && matchingCountries.Any(c => !string.IsNullOrWhiteSpace(c.FullName) && c.FullName.Equals(country.FullName, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add(ErrorMessages.NonUniqueCountryName);
                }

                if (!string.IsNullOrWhiteSpace(country.Alpha2Code) && matchingCountries.Any(c => !string.IsNullOrWhiteSpace(c.Alpha2Code) && c.Alpha2Code.Equals(country.Alpha2Code, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add(ErrorMessages.NonUniqueCountryAbbreviation);
                }
            }

            return errorMessages;
        }
    }
}
