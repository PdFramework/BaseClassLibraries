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

        [Route(Name = Routes.GetCountryNamedRouteName)]
        public override async Task<IHttpActionResult> Get(int id)
        {
            return await base.Get(id);
        }
    }
}
