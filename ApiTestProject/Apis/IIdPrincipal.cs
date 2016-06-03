namespace Apis
{
    using System.Security.Principal;

    public interface IIdPrincipal : IPrincipal
    {
        int Id { get; set; }
    }
}
