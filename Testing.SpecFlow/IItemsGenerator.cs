namespace PeinearyDevelopment.Framework.BaseClassLibraries.Testing.SpecFlow
{
    using Contracts;

    public interface IItemsGenerator<out TContract, in TId> where TContract : IdContractBase<TId>
    {
        TContract GenerateValidItem();
        TContract GenerateValidUpdatedItem(TId id);
    }
}
