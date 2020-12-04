using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class AppVersionMapper
    {
        public static AppVersionDataModel Map(this AppVersionApiModel appVersionApiModel)
        {
            if (appVersionApiModel is null)
            {
                return null;
            }

            return new AppVersionDataModel(appVersionApiModel.Link);
        }
    }
}