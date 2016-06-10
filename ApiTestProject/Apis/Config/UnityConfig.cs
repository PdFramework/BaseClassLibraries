namespace Apis.Config
{
    using DataAccess;
    using DataAccessContracts;

    using Microsoft.Practices.Unity;
    using System.Data.Entity;
    using System.Web;
    using System.Web.Http;
    using Unity.WebApi;

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<DbContext, CountriesDbContext>();
            container.RegisterType<ICountriesDal, CountriesDal>();
            container.RegisterType<ICountrySubdivisionsDal, CountrySubdivisionsDal>();
            container.RegisterType<IIdPrincipal>(new InjectionFactory(u => HttpContext.Current.User));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}