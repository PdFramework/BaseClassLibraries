namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using Contracts;

    public class DateRangeEffectiveContractObject : DateRangeEffectiveContractBase<int>
    {
        public string Property { get; set; }
        public virtual string VirtualProperty { get; set; }
    }
}
