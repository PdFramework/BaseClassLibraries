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

        [Route(Name = Routes.GetCountrySubdivisionNamedRouteName)]
        public override async Task<IHttpActionResult> Get(int id)
        {
            return await base.Get(id);
        }
    }
}
