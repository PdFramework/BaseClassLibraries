﻿namespace Apis.Config
{
    using AutoMapper;
    using AutoMapper.Mappers;

    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; internal set; }

        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddConditionalObjectMapper().Where((s, d) => s.Name == d.Name + "Dto" || s.Name + "Dto" == d.Name);
            });

            Mapper = config.CreateMapper();
        }
    }
}
