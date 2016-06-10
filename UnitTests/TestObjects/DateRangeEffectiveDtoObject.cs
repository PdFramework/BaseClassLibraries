namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using DataAccess.Contracts;

    public class DateRangeEffectiveDtoObject : DateRangeEffectiveDtoBase<int>
    {
        public string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
