using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class WithdrawalMapper
    {
        public static WithdrawalDataModel Map(this WithdrawalApiModel withdrawalApiModel)
        {
            return new WithdrawalDataModel(withdrawalApiModel.Id,
                                           withdrawalApiModel.Amount,
                                           withdrawalApiModel.Status,
                                           withdrawalApiModel.CreatedAt);
        }

        public static WithdrawalDataModel Map(this DataApiModel<WithdrawalApiModel> dataApiModel)
        {
            if (dataApiModel.Data is null)
            {
                return null;
            }

            return new WithdrawalDataModel(dataApiModel.Data.Id,
                                           dataApiModel.Data.Amount,
                                           dataApiModel.Data.Status,
                                           dataApiModel.Data.CreatedAt);
        }
    }
}