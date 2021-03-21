using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class AppVersionMapper
    {
        public static AppVersion Map(this AppVersionDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new AppVersion(dto.Link);
        }
    }
}