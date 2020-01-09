using System;

namespace PrankChat.Mobile.Core.Configuration
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            BaseAddress = "http://stage.prankchat.store/";
            ApiVersion = new Version(1, 0);
        }

        public string BaseAddress { get; }

        public Version ApiVersion { get; }
    }
}
