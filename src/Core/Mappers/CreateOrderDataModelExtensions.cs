using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class CreateOrderDataModelExtensions
    {
        public static CreateOrderApiModel Map(this CreateOrderDataModel model)
        {
            return new CreateOrderApiModel
            {
                Title = model.Title,
                ActiveFor = model.ActiveFor,
                AutoProlongation = model.AutoProlongation,
                Description = model.Description,
                IsHidden = model.IsHidden,
                Price = model.Price
            };
        }
    }
}
