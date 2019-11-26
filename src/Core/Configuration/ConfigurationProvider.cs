namespace PrankChat.Mobile.Core.Configuration
{
    public static class ConfigurationProvider
    {
        public static EnvironmentType CurrentEnvironment { get; set; } = EnvironmentType.Development;

        internal static IConfiguration GetConfiguration()
        {
            switch (CurrentEnvironment)
            {
                case EnvironmentType.Development:
                    return new DevelopmentConfiguration();

                default:
                    return new DevelopmentConfiguration();
            }
        }
    }
}
