namespace Apis.Controllers
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using Contracts;
    using DataAccessContracts;

    using AutoMapper;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix(Routes.CountrySubdivisionV1BaseRoute)]
    public class CountrySubdivisionsController : DateRangeEffectiveBaseApiControllerBase<CountrySubdivision, CountrySubdivisionDto, int>
    {
        public CountrySubdivisionsController(ICountrySubdivisionsDal dal, IMapper mapper, IContractValidator<CountrySubdivision> contractValidator) : base(dal, mapper, contractValidator)
        {
            CreatedRouteName = Routes.GetCountrySubdivisionNamedRouteName;
            ControllerName = Routes.CountrySubdivisionControllerName;
        }

        [HttpGet]
        [Route("{id:int}", Name = Routes.GetCountrySubdivisionNamedRouteName)]
        public override async Task<IHttpActionResult> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [Route("")]
        public override async Task<IHttpActionResult> Post(CountrySubdivision countrySubdivision)
        {
            return await base.Post(countrySubdivision);
        }

        [HttpPut]
        [Route("")]
        public override async Task<IHttpActionResult> Put(CountrySubdivision countrySubdivision)
        {
            return await base.Put(countrySubdivision);
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
