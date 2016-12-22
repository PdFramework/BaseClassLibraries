namespace Apis.Config
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using Contracts;
    using ContractValidators;
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

            container.RegisterType<DbContext, CountriesDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IContractValidator<Country>, CountryValidator>();
            container.RegisterType<ICountriesDal, CountriesDal>();

            container.RegisterType<IContractValidator<CountrySubdivision>, CountrySubdivisionValidator>();
            container.RegisterType<ICountrySubdivisionsDal, CountrySubdivisionsDal>();

            container.RegisterType<IIdPrincipal>(new InjectionFactory(u => HttpContext.Current.User));
            container.RegisterInstance(AutoMapperConfig.Mapper);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}