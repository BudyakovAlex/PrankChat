using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class RecoverPasswordResultMapper
    {
        public static RecoverPasswordResultDataModel Map(this RecoverPasswordResultApiModel recoverPasswordResultApiModel)
        {
            return new RecoverPasswordResultDataModel(recoverPasswordResultApiModel.Result);
        }
    }
}