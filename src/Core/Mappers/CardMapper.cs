using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
