namespace PeinearyDevelopment.Framework.BaseClassLibraries.Contracts
{
    using System.Threading.Tasks;

    public interface IIdClientBase<TContract, in TId> where TContract : IdContractBase<TId>
    {
        Task<TContract> Get(TId id);
        Task<TContract> Create(TContract contract);
        Task<TContract> Update(TContract contract);
        Task Delete(TId id);
    }
}
