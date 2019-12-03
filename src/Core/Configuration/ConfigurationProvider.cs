namespace PrankChat.Mobile.Core.Configuration
{
    public static class ConfigurationProvider
    {
        public static EnvironmentType CurrentEnvironment { get; set; } = EnvironmentType.Development;

        internal static IApplicationConfiguration GetConfiguration()
        {
            switch (CurrentEnvironment)
            {
                case EnvironmentType.Development:
                    return new ApplicationConfiguration();

                default:
                    return new ApplicationConfiguration();
            }
        }
    }
}
