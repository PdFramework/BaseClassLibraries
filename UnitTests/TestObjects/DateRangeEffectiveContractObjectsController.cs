namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using DataAccess.Contracts;
    using Web.Http;

    using System.Threading.Tasks;
    using System.Web.Http;
    using AutoMapper;

    [RoutePrefix("v1/daterangeeffectivecontractobjects")]
    public class DateRangeEffectiveContractObjectsController : DateRangeEffectiveBaseApiControllerBase<DateRangeEffectiveContractObject, DateRangeEffectiveDtoObject, int>
    {
        public DateRangeEffectiveContractObjectsController(IDalBase<DateRangeEffectiveDtoObject, int> dal, IMapper mapper, IContractValidator<DateRangeEffectiveContractObject> contractValidator) : base(dal, mapper, contractValidator)
        {
            CreatedRouteName = StaticTestValues.GetDateRangeEffectiveContractObjectRouteName;
            ControllerName = StaticTestValues.ControllerName;
        }

        [HttpGet]
        [Route(Name = StaticTestValues.GetDateRangeEffectiveContractObjectRouteName)]
        public override async Task<IHttpActionResult> Get(int id)
        {
            return await base.Get(id);
        }
    }
}
