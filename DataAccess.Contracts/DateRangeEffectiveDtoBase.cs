
namespace PeinearyDevelopment.Framework.BaseClassLibraries.DataAccess.Contracts
{
    using System;

    public class DateRangeEffectiveDtoBase<TId> : IdDtoBase<TId>
    {
        public DateTimeOffset EffectiveStartDate { get; set; }
        public DateTimeOffset? EffectiveEndDate { get; set; }
    }
}
