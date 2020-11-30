using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Models.Api;

namespace PrankChat.Mobile.Core.Managers.Orders
{
    public class OrdersManager : IOrdersManager
    {
        private readonly IOrdersService _ordersService;

        public OrdersManager(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderApiModel orderInfo)
        {
            var response = await _ordersService.CreateOrderAsync(orderInfo);
            return response.Map();
        }

        public async Task<PaginationModel<OrderDataModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            var response = await _ordersService.GetUserOwnOrdersAsync(userId, page, pageSize);
            return response.Map();
        }

        public async Task<PaginationModel<OrderDataModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            var response = await _ordersService.GetUserExecuteOrdersAsync(userId, page, pageSize);
            return response.Map();
        }

        public async Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            var response = await _ordersService.GetOrdersAsync(orderFilterType, page, pageSize);
            return response.Map();
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            var response = await _ordersService.GetOrderDetailsAsync(orderId);
            return response.Map();
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            var response = await _ordersService.TakeOrderAsync(orderId);
            return response.Map();
        }

        public async Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
        {
            var response = await _ordersService.GetArbitrationOrdersAsync(filter, page, pageSize);
            return response.Map();
        }

        public async Task<OrderDataModel> CancelOrderAsync(int orderId)
        {
            var response = await _ordersService.CancelOrderAsync(orderId);
            return response.Map();
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            return _ordersService.ComplainOrderAsync(orderId, title, description);
        }

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            var response = await _ordersService.SubscribeOrderAsync(orderId);
            return response.Map();
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            var response = await _ordersService.UnsubscribeOrderAsync(orderId);
            return response.Map();
        }

        public async Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            var response = await _ordersService.ArgueOrderAsync(orderId);
            return response.Map();
        }

        public async Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            var response = await _ordersService.AcceptOrderAsync(orderId);
            return response.Map();
        }

        public async Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var response = await _ordersService.VoteVideoAsync(orderId, isLiked);
            return response.Map();
        }
    }
}