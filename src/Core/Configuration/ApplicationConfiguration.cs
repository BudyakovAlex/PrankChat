using System;

namespace PrankChat.Mobile.Core.Configuration
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        private const string DevAddress = "http://dev.prankchat.store";
        private const string StageAddress = "http://stage.prankchat.store";

        public ApplicationConfiguration()
        {
            BaseAddress = StageAddress;
            ApiVersion = new Version(1, 0);
        }

        public string BaseAddress { get; }

        public Version ApiVersion { get; }
    }
}
