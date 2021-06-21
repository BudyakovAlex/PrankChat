using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Models.Configurations
{
    public class ApplicationConfiguration
    {
        [JsonConstructor]
        public ApplicationConfiguration(string environment, EnvironmentConfiguration[] configurations)
        {
            Environment = environment;
            Configurations = configurations;
        }

        public string Environment { get; }

        public EnvironmentConfiguration[] Configurations { get; }
    }
}