namespace PeinearyDevelopment.Framework.BaseClassLibraries.Contracts
{
    using System;

    public class IdContractBase<TId>
    {
        public TId Id { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public int? LastUpdatedByUserId { get; set; }
        public DateTimeOffset? LastUpdatedOn { get; set; }
    }
}
