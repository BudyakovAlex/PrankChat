using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class ArbitrationOrderMapper
    {
        public static ArbitrationOrderDataModel Map(this ArbitrationOrderApiModel arbitrationOrderApi)
        {
            return new ArbitrationOrderDataModel(arbitrationOrderApi.Id,
                                         arbitrationOrderApi.Price,
                                         arbitrationOrderApi.Title,
                                         arbitrationOrderApi.Description,
                                         arbitrationOrderApi.Status,
                                         arbitrationOrderApi.AutoProlongation,
                                         arbitrationOrderApi.Customer.Map(),
                                         arbitrationOrderApi.Executor.Map(),
                                         arbitrationOrderApi.Video.Map(),
                                         arbitrationOrderApi.Likes,
                                         arbitrationOrderApi.Dislikes,
                                         arbitrationOrderApi.ArbitrationFinishAt);
        }
    }
}
