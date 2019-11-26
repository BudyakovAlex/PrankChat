using System;

namespace PrankChat.Mobile.Core.Configuration
{
    internal class DevelopmentConfiguration : IConfiguration
    {
        public DevelopmentConfiguration()
        {
            BaseAddress = "http://dev.prankchat.store/";
            ApiVersion = new Version(1, 0);
        }

        public string BaseAddress { get; }

        public Version ApiVersion { get; }
    }
}
