using Newtonsoft.Json;

namespace PrankChat.Mobile.Core.Data.Models.Configurations
{
    public class EnvironmentConfiguration
    {
        [JsonConstructor]
        public EnvironmentConfiguration(
            string environmentName,
            string apiUrl,
            string apiVersion,
            string appCenterIosId,
            string appCenterDroidId,
            bool shouldCheckAppUpdate)
        {
            EnvironmentName = environmentName;
            ApiUrl = apiUrl;
            ApiVersion = apiVersion;
            AppCenterIosId = appCenterIosId;
            AppCenterDroidId = appCenterDroidId;
            ShouldCheckAppUpdate = shouldCheckAppUpdate;
        }

        public string EnvironmentName { get; }

        public string ApiUrl { get; }

        public string ApiVersion { get; }

        public string AppCenterIosId { get; }

        public string AppCenterDroidId { get; }

        public bool ShouldCheckAppUpdate { get; }
    }
}