using System;
using System.Collections.Generic;
using AutoMapper;

namespace PrankChat.Mobile.Core.Configuration
{
    internal static class MappingConfig
    {
        private static bool _isConfigure;

        public static IMapper Mapper { get; private set; }

        public static void Configure(IEnumerable<Type> mappingTypes)
        {
            if (_isConfigure)
            {
                return;
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(mappingTypes);
                cfg.CreateMap<byte[], byte[]>().ConvertUsing(new ByteArrayConverter());
            });

            Mapper = config.CreateMapper();

            _isConfigure = true;
        }
    }

    public class ByteArrayConverter : ITypeConverter<byte[], byte[]>
    {
        public byte[] Convert(byte[] source, byte[] destination, ResolutionContext context)
        {
            if (source == null)
            {
                return destination;
            }

            destination = source;
            Array.Copy(source, destination, source.Length);

            return destination;
        }
    }
}
