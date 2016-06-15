namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Security.Principal;

    public interface IIdPrincipal : IPrincipal
    {
        int Id { get; set; }
    }
}
