namespace Apis.Config
{
    using Contracts;
    using DataAccessContracts;

    using AutoMapper;

    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; protected set; }

        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, CountryDto>();
                cfg.CreateMap<CountryDto, Country>();
                cfg.CreateMap<CountrySubdivision, CountrySubdivisionDto>();
                cfg.CreateMap<CountrySubdivisionDto, CountrySubdivision>();
                cfg.CreateMap<CountrySubdivisionType, CountrySubdivisionTypeDto>();
                cfg.CreateMap<CountrySubdivisionTypeDto, CountrySubdivisionType>();
            });

            Mapper = config.CreateMapper();
        }
    }
}
