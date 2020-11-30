using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CardMapper
    {
        public static CardDataModel Map(this CardApiModel cardApiModel)
        {
            return new CardDataModel(cardApiModel.Id,
                                     cardApiModel.Number,
                                     cardApiModel.CardUserName);
        }
    }
}