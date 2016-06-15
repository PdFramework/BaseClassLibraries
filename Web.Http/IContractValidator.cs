namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContractValidator<in TContract>
    {
        Task<IEnumerable<string>> ValidateContract(TContract contract);
    }
}
