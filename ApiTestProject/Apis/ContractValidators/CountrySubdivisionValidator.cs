namespace Apis.ContractValidators
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using Contracts;
    using DataAccessContracts;

    using AutoMapper;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CountrySubdivisionValidator : IContractValidator<CountrySubdivision>
    {
        private ICountriesDal CountriesDal { get; }
        private ICountrySubdivisionsDal CountrySubdivisionsDal { get; }
        private IMapper Mapper { get; }

        public CountrySubdivisionValidator(ICountriesDal countriesDal, ICountrySubdivisionsDal countrySubdivisionsDal, IMapper mapper)
        {
            CountriesDal = countriesDal;
            CountrySubdivisionsDal = countrySubdivisionsDal;
            Mapper = mapper;
        }

        public async Task<IEnumerable<string>> ValidateContract(CountrySubdivision contract)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(contract.Name)) errorMessages.Add(ErrorMessages.CountrySubdivisionNameMissing);

            var country = await CountriesDal.Read(contract.CountryId);
            if (country == null)
            {
                errorMessages.Add(ErrorMessages.InvalidCountryId);
            }

            if (contract.ParentCountrySubdivisionId.HasValue)
            {
                var parentCountrySubdivision = await CountrySubdivisionsDal.Read(contract.ParentCountrySubdivisionId.Value);
                if (parentCountrySubdivision == null)
                {
                    errorMessages.Add(ErrorMessages.InvalidParentCountrySubdivisionId);
                }
                else if (parentCountrySubdivision.CountryId != contract.CountryId)
                {
                    errorMessages.Add(ErrorMessages.MismatchedParentCountrySubdivisionId);
                }
            }

            var matchingCountrySubdivisions = await CountrySubdivisionsDal.GetMatchingCountrySubdivisions(Mapper.Map<CountrySubdivisionDto>(contract));
            if (matchingCountrySubdivisions.Any())
            {
                if (!string.IsNullOrWhiteSpace(contract.Name) && matchingCountrySubdivisions.Any(c => !string.IsNullOrWhiteSpace(c.Name) && c.Name.Equals(contract.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add(ErrorMessages.NonUniqueCountrySubdivisionName);
                }

                if (!string.IsNullOrWhiteSpace(contract.Abbreviation) && matchingCountrySubdivisions.Any(c => !string.IsNullOrWhiteSpace(c.Abbreviation) && c.Abbreviation.Equals(contract.Abbreviation, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add(ErrorMessages.NonUniqueCountrySubdivisionAbbreviation);
                }
            }

            return errorMessages;
        }
    }
}
