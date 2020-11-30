using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class AppVersionMapper
    {
        public static AppVersionDataModel Map(this AppVersionApiModel appVersionApiModel)
        {
            return new AppVersionDataModel(appVersionApiModel.Link);
        }
    }
}