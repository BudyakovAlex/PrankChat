using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Common;
using PrankChat.Mobile.Core.Managers.Common;
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

        public Task<AppVersionDataModel> CheckAppVersionAsync()
        {
            return _versionService.CheckAppVersionAsync();
        }
    }
}