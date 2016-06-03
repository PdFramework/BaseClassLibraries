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

    [RoutePrefix("v1/countries")]
    public class CountriesController : BaseApiController
    {
        private ICountriesDal CountriesDal { get; }

        public CountriesController(ICountriesDal countriesDal)
        {
            CountriesDal = countriesDal;
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetCountry")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                return Ok(AutoMapperConfig.Mapper.Map<Country>(await CountriesDal.Read(id)));
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post(Country country)
        {
            var validationErrors = await ValidateCountry(country);
            if (validationErrors != null && validationErrors.Any()) return BadRequest(string.Join("\n", validationErrors));

            country.CreatedByUserId = IdPrincipal.Id;
            country.CreatedOn = DateTimeOffset.UtcNow;
            country.EffectiveStartDate = country.EffectiveStartDate == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : country.EffectiveStartDate;
            country.LastUpdatedByUserId = null;
            country.LastUpdatedOn = null;

            var createdCountryDto = await CountriesDal.Create(AutoMapperConfig.Mapper.Map<CountryDto>(country));
            var createdCountry = AutoMapperConfig.Mapper.Map<Country>(createdCountryDto);

            return Created(Url.Link("GetCountry", new { id = createdCountry.Id, controller = "Countries" }), createdCountry);
        }

        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Put(Country country)
        {
            var validationErrors = await ValidateCountry(country);
            if (validationErrors != null && validationErrors.Any()) return BadRequest(string.Join("\n", validationErrors));

            try
            {
                country.LastUpdatedByUserId = IdPrincipal.Id;
                country.LastUpdatedOn = DateTimeOffset.UtcNow;

                var updatedCountryDto = await CountriesDal.Update(AutoMapperConfig.Mapper.Map<CountryDto>(country));
                var updatedCountry = AutoMapperConfig.Mapper.Map<Country>(updatedCountryDto);
                return Ok(updatedCountry);
            }
            catch (ObjectNotFoundException)
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
                var country = await CountriesDal.Read(id);

                country.LastUpdatedByUserId = IdPrincipal.Id;
                country.LastUpdatedOn = DateTimeOffset.UtcNow;
                country.EffectiveEndDate = DateTimeOffset.UtcNow;

                await CountriesDal.Update(country);
                
                return Ok();
            }
            catch (ObjectNotFoundException)
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
                await CountriesDal.Delete(id);
                return this.NoContent();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        private async Task<List<string>> ValidateCountry(Country country)
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(country.FullName)) errorMessages.Add("Country's name can't be blank.");
            if (string.IsNullOrWhiteSpace(country.Alpha2Code)) errorMessages.Add("Country's abbreviation can't be blank.");
            if (country.EffectiveEndDate != null && country.EffectiveEndDate <= country.EffectiveStartDate) errorMessages.Add("Country's effective date range is invalid. EffectiveEndDate must be null or greater than the EffectiveStartDate.");

            var matchingCountries = await CountriesDal.GetMatchingCountries(AutoMapperConfig.Mapper.Map<CountryDto>(country));
            if(matchingCountries.Any())
            {
                if (!string.IsNullOrWhiteSpace(country.FullName) && matchingCountries.Any(c => !string.IsNullOrWhiteSpace(c.FullName) && c.FullName.Equals(country.FullName, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add("Country name needs to be unique.");
                }

                if (!string.IsNullOrWhiteSpace(country.Alpha2Code) && matchingCountries.Any(c => !string.IsNullOrWhiteSpace(c.Alpha2Code) && c.Alpha2Code.Equals(country.Alpha2Code, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessages.Add("Country abbreviation needs to be unique.");
                }
            }

            return errorMessages;
        }
    }
}
