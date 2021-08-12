using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Services.Network.Http.Common;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Managers.Common
{
    public class VersionManager : IVersionManager
    {
        private readonly IVersionService _versionService;
        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;

        public VersionManager(IVersionService versionService, IEnvironmentConfigurationProvider environmentConfigurationProvider)
        {
            _versionService = versionService;
            _environmentConfigurationProvider = environmentConfigurationProvider;
        }

        public async Task<AppVersion> CheckAppVersionAsync()
        {
            if (!_environmentConfigurationProvider.Environment.ShouldCheckAppUpdate)
            {
                return null;
            }

            var response = await _versionService.CheckAppVersionAsync();
            return response.Map();
        }
    }
}