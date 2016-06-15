namespace PeinearyDevelopment.Framework.BaseClassLibraries.UnitTests.TestObjects
{
    using AutoMapper;

    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; internal set; }

        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DateRangeEffectiveDtoObject, DateRangeEffectiveContractObject>();
                cfg.CreateMap<DateRangeEffectiveContractObject, DateRangeEffectiveDtoObject>();
            });

            Mapper = config.CreateMapper();
        }
    }
}
