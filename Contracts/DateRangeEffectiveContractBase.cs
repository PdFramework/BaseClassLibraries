namespace PeinearyDevelopment.Framework.BaseClassLibraries.Contracts
{
    using System;

    public class DateRangeEffectiveContractBase<TId> : IdBase<TId>
    {
        public DateTimeOffset EffectiveStartDate { get; set; }
        public DateTimeOffset? EffectiveEndDate { get; set; }
    }
}
