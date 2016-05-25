namespace Apis.Controllers
{
    using Config;
    using Contracts;
    using DataAccessContracts;

    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("v1/countrysubdivisions")]
    public class CountrySubdivisionsController : ApiController
    {
        private ICountriesDal CountriesDal { get; }
        private ICountrySubdivisionsDal CountrySubdivisionsDal { get; }

        public CountrySubdivisionsController(ICountriesDal countriesDal, ICountrySubdivisionsDal countrySubdivisionsDal)
        {
            CountriesDal = countriesDal;
            CountrySubdivisionsDal = countrySubdivisionsDal;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                return Ok(AutoMapperConfig.Mapper.Map<CountrySubdivision>(await CountrySubdivisionsDal.Read(id)));
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(CountrySubdivision countrySubdivision)
        {
            var validationErrors = await ValidateCountrySubdivision(countrySubdivision);
            if (validationErrors != null && validationErrors.Any()) return BadRequest(string.Join("\n", validationErrors));

            var createdCountrySubdivisionDto = await CountrySubdivisionsDal.Create(AutoMapperConfig.Mapper.Map<CountrySubdivisionDto>(countrySubdivision));
            var createdCountrySubdivision = AutoMapperConfig.Mapper.Map<CountrySubdivision>(createdCountrySubdivisionDto);
            return Ok(createdCountrySubdivision);
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Put(CountrySubdivision countrySubdivision)
        {
            var validationErrors = await ValidateCountrySubdivision(countrySubdivision);
            if (validationErrors != null && validationErrors.Any()) return BadRequest(string.Join("\n", validationErrors));

            try
            {
                var updatedCountrySubdivisionDto = await CountrySubdivisionsDal.Update(AutoMapperConfig.Mapper.Map<CountrySubdivisionDto>(countrySubdivision));
                var updatedCountrySubdivision = AutoMapperConfig.Mapper.Map<Country>(updatedCountrySubdivisionDto);
                return Ok(updatedCountrySubdivision);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> SoftDelete(int id)
        {
            try
            {
                var countrySubdivision = await CountrySubdivisionsDal.Read(id);
                countrySubdivision.EffectiveEndDate = DateTimeOffset.UtcNow;
                await CountrySubdivisionsDal.Update(countrySubdivision);

                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id:int}/force")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                await CountrySubdivisionsDal.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        private async Task<List<string>> ValidateCountrySubdivision(CountrySubdivision countrySubdivision)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(countrySubdivision.Name)) errorMessages.Add("Country subdivision's name can't be blank.");

            var country = await CountriesDal.Read(countrySubdivision.CountryId);
            if (country == null)
            {
                errorMessages.Add("Invalid country id.");
            }

            if (countrySubdivision.ParentCountrySubdivisionId.HasValue)
            {
                var parentCountrySubdivision = await CountrySubdivisionsDal.Read(countrySubdivision.ParentCountrySubdivisionId.Value);
                if (parentCountrySubdivision == null)
                {
                    errorMessages.Add("Invalid parent country subdivision id.");
                }
                else if (parentCountrySubdivision.CountryId != countrySubdivision.CountryId)
                {
                    errorMessages.Add("The parent country subdivision chosen is part of a different country than the country chosen for this subdivision.");
                }
            }

            var matchingCountrySubdivisions = await CountrySubdivisionsDal.GetMatchingCountrySubdivisions(AutoMapperConfig.Mapper.Map<CountrySubdivisionDto>(countrySubdivision));
            if (matchingCountrySubdivisions.Any())
            {
                if (!string.IsNullOrWhiteSpace(countrySubdivision.Name) && matchingCountrySubdivisions.Any(c => !string.IsNullOrWhiteSpace(c.Name) && c.Name.Equals(countrySubdivision.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add("Country subdivision name needs to be unique.");
                }

                if (!string.IsNullOrWhiteSpace(countrySubdivision.Abbreviation) && matchingCountrySubdivisions.Any(c => !string.IsNullOrWhiteSpace(c.Abbreviation) && c.Abbreviation.Equals(countrySubdivision.Abbreviation, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add("Country subdivision abbreviation needs to be unique.");
                }
            }

            return errorMessages;
        }
    }
}
