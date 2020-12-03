using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class ArbitrationOrderMapper
    {
        public static ArbitrationOrderDataModel Map(this ArbitrationOrderApiModel arbitrationOrderApi)
        {
            if (arbitrationOrderApi.Customer is null ||
                arbitrationOrderApi.Executor is null ||
                arbitrationOrderApi.Video is null)
            {
                return null;
            }

            return new ArbitrationOrderDataModel(arbitrationOrderApi.Id,
                                                 arbitrationOrderApi.Price,
                                                 arbitrationOrderApi.Title,
                                                 arbitrationOrderApi.Description,
                                                 arbitrationOrderApi.Status,
                                                 arbitrationOrderApi.AutoProlongation,
                                                 arbitrationOrderApi.Customer?.Map(),
                                                 arbitrationOrderApi.Executor?.Map(),
                                                 arbitrationOrderApi.Video?.Map(),
                                                 arbitrationOrderApi.Likes,
                                                 arbitrationOrderApi.Dislikes,
                                                 arbitrationOrderApi.ArbitrationFinishAt);
        }
    }
}