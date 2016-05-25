namespace Apis
{
    using System.Web.Http;

    public class BaseApiController : ApiController
    {
        private IIdPrincipal _idPrincipal = null;

        public IIdPrincipal IdPrincipal
        {
            get
            {
                return _idPrincipal ?? new IdPrincipal("TestUser") { Id = -365 };
            }
            set
            {
                _idPrincipal = value;
            }
        }
    }
}
