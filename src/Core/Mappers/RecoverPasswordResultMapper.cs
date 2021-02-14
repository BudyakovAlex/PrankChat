using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class RecoverPasswordResultMapper
    {
        public static RecoverPasswordResult Map(this RecoverPasswordResultDto dto)
        {
            return new RecoverPasswordResult(dto.Result);
        }
    }
}