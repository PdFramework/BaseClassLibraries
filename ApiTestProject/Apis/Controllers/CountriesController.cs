namespace Apis.Controllers
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using Contracts;
    using DataAccessContracts;

    using AutoMapper;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix(Routes.CountryV1BaseRoute)]
    public class CountriesController : DateRangeEffectiveBaseApiControllerBase<Country, CountryDto, int>
    {
        public CountriesController(ICountriesDal dal, IMapper mapper, IContractValidator<Country> contractValidator) : base(dal, mapper, contractValidator)
        {
            CreatedRouteName = Routes.GetCountryNamedRouteName;
            ControllerName = Routes.CountryControllerName;
        }

        [HttpGet]
        [Route("{id:int}", Name = Routes.GetCountryNamedRouteName)]
        public override async Task<IHttpActionResult> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [Route("")]
        public override async Task<IHttpActionResult> Post(Country country)
        {
            return await base.Post(country);
        }

        [HttpPut]
        [Route("")]
        public override async Task<IHttpActionResult> Put(Country country)
        {
            return await base.Put(country);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public override async Task<IHttpActionResult> SoftDelete(int id)
        {
            return await base.SoftDelete(id);
        }

        [HttpDelete]
        [Route("{id:int}/force")]
        public override async Task<IHttpActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
