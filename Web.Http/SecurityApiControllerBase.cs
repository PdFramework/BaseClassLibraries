namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Web.Http;

    public class SecurityApiControllerBase : ApiController
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
