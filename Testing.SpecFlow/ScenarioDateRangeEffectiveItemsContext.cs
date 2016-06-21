namespace PeinearyDevelopment.Framework.BaseClassLibraries.Testing.SpecFlow
{
    using Contracts;

    using System;
    using System.Globalization;
    using System.Net.Http;

    public class ScenarioDateRangeValidItemsContext<T, TId> : ScenarioItemsContext<T, TId> where T : DateRangeEffectiveContractBase<TId>
    {
        public string ForceDeleteItemPathFormat { get; set; }

        public void ForceDeleteItem()
        {
            var createdItem = GetStatefulItem(ItemContextState.Created);
            ForceDeleteItem(createdItem.Id);
            SetStatefulItem(ItemContextState.Deleted, createdItem);
            TestsGeneratedItems.Remove(createdItem);
        }

        public void Cleanup()
        {
            foreach (var testItemToCleanup in TestsGeneratedItems)
            {
                ForceDeleteItem(testItemToCleanup.Id);
            }
        }

        public void ForceDeleteItem(TId id)
        {
            using (var client = new HttpClient())
            {
                using (var response = client.DeleteAsync(new Uri(string.Format(CultureInfo.InvariantCulture, ForceDeleteItemPathFormat, id))).GetAwaiter().GetResult())
                {
                    if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.Content.ToString());
                }
            }
        }
    }
}
