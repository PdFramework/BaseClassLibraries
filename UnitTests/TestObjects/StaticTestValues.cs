namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using System;

    public static class StaticTestValues
    {
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

        public const int InvalidId1 = -1;
        public const int ValidId1 = 1;
        public const int ValidId2 = 2;
        public const string ValidProperty1 = "ValidProperty1";
        public const string ValidProperty2 = "ValidProperty2";
        public const string ValidVirtualProperty1 = "ValidVirtualProperty1";
        public const string ValidVirtualProperty2 = "ValidVirtualProperty2";
        public static DateTimeOffset EffectiveStartDateTimeOffset1 { get; }
        public static DateTimeOffset EffectiveStartDateTimeOffset2 { get; }
        public static DateTimeOffset? EffectiveEndDateTimeOffset1 { get; }
        public static DateTimeOffset? EffectiveEndDateTimeOffset2 { get; }
        public const int CreatedByUserId1 = 3;
        public const int CreatedByUserId2 = 4;
        public static DateTimeOffset CreatedOnDateTimeOffset1 { get; }
        public static DateTimeOffset CreatedOnDateTimeOffset2 { get; }
        public static int? LastUpdatedByUserId1 = null;
        public static int? LastUpdatedByUserId2 = 5;
        public static DateTimeOffset? LastUpdatedOnDateTimeOffset1 { get; }
        public static DateTimeOffset? LastUpdatedOnDateTimeOffset2 { get; }

        public const string ControllerName = "DateRangeEffectiveContractObjects";
        public const string GetDateRangeEffectiveContractObjectRouteName = "GetDateRangeEffectiveContractObject";

        public const string ContractValidatorErrorMessage1 = "Contract failed validation.";
        public const string ContractValidatorErrorMessage2 = "Contract is bad.";
    }
}
