namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Security.Principal;

    public class IdPrincipal : IIdPrincipal
    {
        public IdPrincipal(string name)
        {
            Identity = new GenericIdentity(name);
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public IIdentity Identity { get; }
        public int Id { get; set; }
    }
}
