﻿namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using DataAccess.Contracts;

    public class DateRangeEffectiveDtoObject : DateRangeEffectiveDtoBase<int>
    {
        public string Property { get; set; }
        public virtual string VirtualProperty { get; set; }
    }
}
