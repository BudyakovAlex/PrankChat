using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class PaymentMapper
    {
        public static PaymentDataModel Map(this PaymentApiModel paymentApiModel)
        {
            return new PaymentDataModel(paymentApiModel.Id,
                                        paymentApiModel.Amount,
                                        paymentApiModel.Provider,
                                        paymentApiModel.Status,
                                        paymentApiModel.PaymentLink);
        }

        public static PaymentDataModel Map(this DataApiModel<PaymentApiModel> dataApiModel)
        {
            if (dataApiModel.Data is null)
            {
                return null;
            }

            return new PaymentDataModel(dataApiModel.Data.Id,
                                        dataApiModel.Data.Amount,
                                        dataApiModel.Data.Provider,
                                        dataApiModel.Data.Status,
                                        dataApiModel.Data.PaymentLink);
        }
    }
}