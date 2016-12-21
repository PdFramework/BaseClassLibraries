namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Noop", Justification = "Common short hand in software development for something that performs no operation.")]
    public class NoopContractValidator<TContract> : IContractValidator<TContract>
    {
        public async Task<IEnumerable<string>> ValidateContract(TContract contract)
        {
            return await Task.Factory.StartNew(() => new string[0]);
        }
    }
}
