using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrankChat.Mobile.Core.Data.Models.Configurations;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrankChat.Mobile.Core.Providers.Configuration
{
    public class EnvironmentConfigurationProvider : IEnvironmentConfigurationProvider
    {
        public EnvironmentConfigurationProvider()
        {
            Initialize();
            SetupPeriods();
        }

        public EnvironmentConfiguration Environment { get; private set; }

        public IReadOnlyList<Period> Periods { get; private set; }

        private void Initialize()
        {
            var jsonContent = LoadConfig();
            LogConfigWarningIfNeed(jsonContent);

            try
            {
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return;
                }

                var appConfig = JsonConvert.DeserializeObject<ApplicationConfiguration>(jsonContent);
                Environment = appConfig.Configurations.FirstOrDefault(config => config.EnvironmentName == appConfig.Environment);
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        private void SetupPeriods()
        {
            Periods = new List<Period>()
            {
                new Period(1, $"1 {Resources.Create_Order_Hour}"),
                new Period(2, $"2 {Resources.Create_Orders_Hours_Singular}"),
                new Period(4, $"4 {Resources.Create_Orders_Hours_Singular}"),
                new Period(6, $"6 {Resources.Create_Orders_Hours_Plural}"),
                new Period(8, $"8 {Resources.Create_Orders_Hours_Plural}"),
                new Period(12, $"12 {Resources.Create_Orders_Hours_Plural}"),
                new Period(16, $"16 {Resources.Create_Orders_Hours_Plural}"),
                new Period(20, $"20 {Resources.Create_Orders_Hours_Plural}"),
                new Period(24, $"24 {Resources.Create_Orders_Hours_Singular}"),
                new Period(32, $"32 {Resources.Create_Orders_Hours_Singular}"),
                new Period(36, $"36 {Resources.Create_Orders_Hours_Plural}"),
                new Period(40, $"40 {Resources.Create_Orders_Hours_Plural}"),
                new Period(44, $"44 {Resources.Create_Orders_Hours_Singular}"),
                new Period(48, $"48 {Resources.Create_Orders_Hours_Plural}"),
            };
        }

        private string LoadConfig()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{nameof(PrankChat)}.{nameof(Mobile)}.{nameof(Core)}.{Constants.Configuration.AppConfigFileName}");
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            };
        }

        private void LogConfigWarningIfNeed(string configJson)
        {
            try
            {
                var currentConfig = LoadCurrentConfigFromToken(configJson);
                if (currentConfig is null)
                {
                    return;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Found not resolved customer properties in AppConfig.json:");

                var notResolvedPropertiesCount = 0;
                var properties = typeof(Environment).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    var currentConfigProperty = currentConfig[property.Name];
                    if (currentConfigProperty is null)
                    {
                        stringBuilder.AppendLine(property.Name);
                        ++notResolvedPropertiesCount;
                    }
                }

                if (notResolvedPropertiesCount == 0)
                {
                    return;
                }

                System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private JToken LoadCurrentConfigFromToken(string configJson)
        {
            try
            {
                var jsonObject = JObject.Parse(configJson);
                var environmentName = jsonObject[nameof(ApplicationConfiguration.Environment)];
                var configs = jsonObject[nameof(ApplicationConfiguration.Configurations)];

                var currentConfig = configs.Where(config => config[nameof(EnvironmentConfiguration.EnvironmentName)].Value<string>() == environmentName.Value<string>())
                                           .ToList()
                                           .FirstOrDefault();
                return currentConfig;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
