using PrankChat.Mobile.Core.Data.Models.Configurations;
using PrankChat.Mobile.Core.Models.Data;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Providers.Configuration
{
    public interface IEnvironmentConfigurationProvider
    {
        public EnvironmentConfiguration Environment { get; }

        public IReadOnlyList<Period> Periods { get; }
    }
}