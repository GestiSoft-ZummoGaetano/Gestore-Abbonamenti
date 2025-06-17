using System;
using AutoMapper;

namespace GestoreAbbonamenti.Logic.Mapping
{
    public static class MapperWrapper
    {
        private static IMapper? Mapper { get; set; }

        public static MapperConfiguration? Configuration { get; set; }
        public static TDestination Map<TDestination>(object source)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map<TDestination>(source);
        }

        public static TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map<TDestination>(source, opts);
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map(source, opts);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map(source, destination);
        }

        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map(source, destination, opts);
        }

        public static object Map(object source, Type sourceType, Type destinationType)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map(source, sourceType, destinationType);
        }

        public static object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map(source, destination, sourceType, destinationType);
        }

        public static object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            if (Mapper == null)
            {
                throw new Exception("MapperWrapper - Mapper property was not initialized. MapperWrapper must be initialized before usage!");
            }

            return Mapper.Map(source, destination, sourceType, destinationType, opts);
        }

        public static void Initialize(Action<IMapperConfigurationExpression> configure)
        {
            Configuration = new MapperConfiguration(configure);
            Mapper = new AutoMapper.Mapper(Configuration);
        }
    }
}

