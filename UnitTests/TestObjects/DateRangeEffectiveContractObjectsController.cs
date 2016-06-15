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
        [Route("{id:int}", Name = StaticTestValues.GetDateRangeEffectiveContractObjectRouteName)]
        public override async Task<IHttpActionResult> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [Route("")]
        public override async Task<IHttpActionResult> Post(DateRangeEffectiveContractObject contract)
        {
            return await base.Post(contract);
        }

        [HttpPut]
        [Route("")]
        public override async Task<IHttpActionResult> Put(DateRangeEffectiveContractObject contract)
        {
            return await base.Put(contract);
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
