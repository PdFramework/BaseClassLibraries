namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using System;

    public static class StaticTestValues
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static StaticTestValues()
        {
            EffectiveStartDateTimeOffset1 = DateTimeOffset.UtcNow.AddYears(-1).AddDays(1);
            EffectiveStartDateTimeOffset2 = DateTimeOffset.UtcNow.AddYears(-1).AddDays(2);
            EffectiveEndDateTimeOffset1 = null;
            EffectiveEndDateTimeOffset2 = DateTimeOffset.UtcNow.AddDays(-1);
            CreatedOnDateTimeOffset1 = DateTimeOffset.UtcNow.AddYears(-1);
            CreatedOnDateTimeOffset2 = DateTimeOffset.UtcNow.AddYears(-2);
            LastUpdatedOnDateTimeOffset1 = null;
            LastUpdatedOnDateTimeOffset2 = DateTimeOffset.UtcNow;
        }

        public const int InvalidDateRangeEffectiveDtoObjectId1 = -1;
        public const int ValidDateRangeEffectiveDtoObjectId1 = 1;
        public const int ValidDateRangeEffectiveDtoObjectId2 = 2;
        public const string ValidName1 = "ValidName1";
        public const string ValidName2 = "ValidName2";
        public const string ValidDescription1 = "ValidDescription1";
        public const string ValidDescription2 = "ValidDescription2";
        public static DateTimeOffset EffectiveStartDateTimeOffset1 { get; }
        public static DateTimeOffset EffectiveStartDateTimeOffset2 { get; }
        public static DateTimeOffset? EffectiveEndDateTimeOffset1 { get; }
        public static DateTimeOffset? EffectiveEndDateTimeOffset2 { get; }
        public const int CreatedByUserId1 = 3;
        public const int CreatedByUserId2 = 4;
        public static DateTimeOffset CreatedOnDateTimeOffset1 { get; }
        public static DateTimeOffset CreatedOnDateTimeOffset2 { get; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static int? LastUpdatedByUserId1 = null;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static int? LastUpdatedByUserId2 = 5;
        public static DateTimeOffset? LastUpdatedOnDateTimeOffset1 { get; }
        public static DateTimeOffset? LastUpdatedOnDateTimeOffset2 { get; }
    }
}
