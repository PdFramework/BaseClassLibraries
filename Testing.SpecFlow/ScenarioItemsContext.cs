namespace PeinearyDevelopment.Framework.BaseClassLibraries.Testing.SpecFlow
{
    using Contracts;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ScenarioItemsContext<T, TId> where T : IdContractBase<TId>
    {
        public IIdClientBase<T, TId> ItemsClient { get; set; }
        public IItemsGenerator<T, TId> ItemsGenerator { get; set; }
        public Collection<T> TestsGeneratedItems { get; }
        internal readonly Dictionary<ItemContextState, T> StatefulItems;

        public ScenarioItemsContext()
        {
            TestsGeneratedItems = new Collection<T>();
            StatefulItems = new Dictionary<ItemContextState, T>();
        }

        public T GetStatefulItem(ItemContextState itemContextState)
        {
            return StatefulItems[itemContextState];
        }

        public void SetStatefulItem(ItemContextState itemContextState, T item)
        {
            StatefulItems[itemContextState] = item;

            if(itemContextState == ItemContextState.Created) TestsGeneratedItems.Add(item);
        }

        public void CreateItem()
        {
            SetStatefulItem(ItemContextState.Created, ItemsClient.Create(GetStatefulItem(ItemContextState.CurrentTestState)).GetAwaiter().GetResult());
        }

        public void GetItem(ItemContextState itemContextState)
        {
            SetStatefulItem(ItemContextState.Retrieved, ItemsClient.Get(GetStatefulItem(itemContextState).Id).GetAwaiter().GetResult());
        }

        public void UpdateItem()
        {
            SetStatefulItem(ItemContextState.Updated, ItemsClient.Update(GetStatefulItem(ItemContextState.CurrentTestState)).GetAwaiter().GetResult());
        }

        public void DeleteItem()
        {
            ItemsClient.Delete(GetStatefulItem(ItemContextState.Created).Id).GetAwaiter().GetResult();
            GetItem(ItemContextState.Created);
            SetStatefulItem(ItemContextState.Deleted, GetStatefulItem(ItemContextState.Retrieved));
        }

        public void GenerateValidItem()
        {
            SetStatefulItem(ItemContextState.CurrentTestState, ItemsGenerator.GenerateValidItem());
        }

        public void GenerateValidUpdatedItem()
        {
            SetStatefulItem(ItemContextState.CurrentTestState, ItemsGenerator.GenerateValidUpdatedItem(GetStatefulItem(ItemContextState.Created).Id));
        }
    }
}
