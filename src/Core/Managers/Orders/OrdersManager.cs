using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;

namespace PrankChat.Mobile.Core.Managers.Orders
{
    public class OrdersManager : IOrdersManager
    {
        private readonly IOrdersService _ordersService;

        public OrdersManager(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            return _ordersService.CreateOrderAsync(orderInfo);
        }

        public Task<PaginationModel<OrderDataModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            return _ordersService.GetUserOwnOrdersAsync(userId, page, pageSize);
        }

        public Task<PaginationModel<OrderDataModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            return _ordersService.GetUserExecuteOrdersAsync(userId, page, pageSize);
        }

        public Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            return _ordersService.GetOrdersAsync(orderFilterType, page, pageSize);
        }

        public Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            return _ordersService.GetOrderDetailsAsync(orderId);
        }

        public Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            return _ordersService.TakeOrderAsync(orderId);
        }

        public Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
        {
            return _ordersService.GetArbitrationOrdersAsync(filter, page, pageSize);
        }

        public Task<OrderDataModel> CancelOrderAsync(int orderId)
        {
            return _ordersService.CancelOrderAsync(orderId);
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            return _ordersService.ComplainOrderAsync(orderId, title, description);
        }

        public Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            return _ordersService.SubscribeOrderAsync(orderId);
        }

        public Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            return _ordersService.UnsubscribeOrderAsync(orderId);
        }

        public Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            return _ordersService.ArgueOrderAsync(orderId);
        }

        public Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            return _ordersService.AcceptOrderAsync(orderId);
        }

        public Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            return _ordersService.VoteVideoAsync(orderId, isLiked);
        }
    }
}