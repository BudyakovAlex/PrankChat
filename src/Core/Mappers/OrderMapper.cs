using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class OrderMapper
    {
        public static OrderDataModel Map(this OrderApiModel orderApiModel)
        {
            return new OrderDataModel(orderApiModel.Id,
                                      orderApiModel.Price,
                                      orderApiModel.Title,
                                      orderApiModel.Description,
                                      orderApiModel.Status,
                                      orderApiModel.OrderCategory,
                                      orderApiModel.ActiveTo,
                                      orderApiModel.DurationInHours,
                                      orderApiModel.AutoProlongation,
                                      orderApiModel.CreatedAt,
                                      orderApiModel.TakenToWorkAt,
                                      orderApiModel.VideoUploadedAt,
                                      orderApiModel.ArbitrationFinishAt,
                                      orderApiModel.CloseOrderAt,
                                      orderApiModel.Customer.Map(),
                                      orderApiModel.Executor.Map(),
                                      orderApiModel.Video.Map(),
                                      orderApiModel.MyArbitrationValue,
                                      orderApiModel.NegativeArbitrationValuesCount,
                                      orderApiModel.PositiveArbitrationValuesCount);
        }

        public static OrderDataModel Map(this DataApiModel<OrderApiModel> dataApiModel)
        {
            return new OrderDataModel(dataApiModel.Data.Id,
                                      dataApiModel.Data.Price,
                                      dataApiModel.Data.Title,
                                      dataApiModel.Data.Description,
                                      dataApiModel.Data.Status,
                                      dataApiModel.Data.OrderCategory,
                                      dataApiModel.Data.ActiveTo,
                                      dataApiModel.Data.DurationInHours,
                                      dataApiModel.Data.AutoProlongation,
                                      dataApiModel.Data.CreatedAt,
                                      dataApiModel.Data.TakenToWorkAt,
                                      dataApiModel.Data.VideoUploadedAt,
                                      dataApiModel.Data.ArbitrationFinishAt,
                                      dataApiModel.Data.CloseOrderAt,
                                      dataApiModel.Data.Customer.Map(),
                                      dataApiModel.Data.Executor.Map(),
                                      dataApiModel.Data.Video.Map(),
                                      dataApiModel.Data.MyArbitrationValue,
                                      dataApiModel.Data.NegativeArbitrationValuesCount,
                                      dataApiModel.Data.PositiveArbitrationValuesCount);
        }

        public static CreateOrderDataModel Map(this CreateOrderApiModel createOrderApiModel)
        {
            return new CreateOrderDataModel(createOrderApiModel.Title,
                                            createOrderApiModel.Description,
                                            createOrderApiModel.Price,
                                            createOrderApiModel.ActiveFor,
                                            createOrderApiModel.AutoProlongation,
                                            createOrderApiModel.IsHidden);
        }
    }
}
