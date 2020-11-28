using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Orders
{
    public class OrdersManager : IOrdersManager
    {
        private readonly IOrdersService _ordersService;

        public OrdersManager(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            return await _ordersService.CreateOrderAsync(orderInfo);
        }

        public async Task<PaginationModel<OrderDataModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            return await _ordersService.GetUserOwnOrdersAsync(userId, page, pageSize);
        }

        public async Task<PaginationModel<OrderDataModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            return await _ordersService.GetUserExecuteOrdersAsync(userId, page, pageSize);
        }

        public async Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            return await _ordersService.GetOrdersAsync(orderFilterType, page, pageSize);
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            return await _ordersService.GetOrderDetailsAsync(orderId);
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            return await _ordersService.TakeOrderAsync(orderId);
        }

        public async Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
        {
            return await _ordersService.GetArbitrationOrdersAsync(filter, page, pageSize);
        }

        public async Task<OrderDataModel> CancelOrderAsync(int orderId)
        {
            return await _ordersService.CancelOrderAsync(orderId);
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            return _ordersService.ComplainOrderAsync(orderId, title, description);
        }

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            return await _ordersService.SubscribeOrderAsync(orderId);
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            return await _ordersService.UnsubscribeOrderAsync(orderId);
        }

        public async Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            return await _ordersService.ArgueOrderAsync(orderId);
        }

        public async Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            return await _ordersService.AcceptOrderAsync(orderId);
        }

        public async Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            return await _ordersService.VoteVideoAsync(orderId, isLiked);
        }
    }
}