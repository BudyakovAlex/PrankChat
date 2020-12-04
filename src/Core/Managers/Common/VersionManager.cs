using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common;
using PrankChat.Mobile.Core.Managers.Common;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Managers.Common
{
    public class VersionManager : IVersionManager
    {
        private readonly IVersionService _versionService;

        public VersionManager(IVersionService versionService)
        {
            _versionService = versionService;
        }

        public async Task<AppVersionDataModel> CheckAppVersionAsync()
        {
            var response = await _versionService.CheckAppVersionAsync();
            return response.Map();
        }
    }
}