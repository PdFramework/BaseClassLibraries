namespace PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess.Contracts
{
    using System.Threading.Tasks;

    public interface IDalBase<TDto, in TId> where TDto : IdDtoBase<TId>
    {
        Task<TDto> Create(TDto dto);
        Task<TDto> Read(TId id);
        Task<TDto> Update(TDto dto);
        Task Delete(TId id);
    }
}
