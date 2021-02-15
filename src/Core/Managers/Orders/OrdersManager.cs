using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
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

        public async Task<Order> CreateOrderAsync(CreateOrder createOrder)
        {
            var apiModel = new CreateOrderDto()
            {
                Title = createOrder.Title,
                ActiveFor = createOrder.ActiveFor,
                AutoProlongation = createOrder.AutoProlongation,
                Description = createOrder.Description,
                IsHidden = createOrder.IsHidden,
                Price = createOrder.Price
            };

            var response = await _ordersService.CreateOrderAsync(apiModel);
            return response.Map();
        }

        public async Task<Pagination<Order>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            var response = await _ordersService.GetUserOwnOrdersAsync(userId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<Order>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            var response = await _ordersService.GetUserExecuteOrdersAsync(userId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<Order>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            var response = await _ordersService.GetOrdersAsync(orderFilterType, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            var response = await _ordersService.GetOrderDetailsAsync(orderId);
            return response.Map();
        }

        public async Task<Order> TakeOrderAsync(int orderId)
        {
            var response = await _ordersService.TakeOrderAsync(orderId);
            return response.Map();
        }

        public async Task<Pagination<ArbitrationOrder>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
        {
            var response = await _ordersService.GetArbitrationOrdersAsync(filter, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Order> CancelOrderAsync(int orderId)
        {
            var response = await _ordersService.CancelOrderAsync(orderId);
            return response.Map();
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            return _ordersService.ComplainOrderAsync(orderId, title, description);
        }

        public async Task<Order> SubscribeOrderAsync(int orderId)
        {
            var response = await _ordersService.SubscribeOrderAsync(orderId);
            return response.Map();
        }

        public async Task<Order> UnsubscribeOrderAsync(int orderId)
        {
            var response = await _ordersService.UnsubscribeOrderAsync(orderId);
            return response.Map();
        }

        public async Task<Order> ArgueOrderAsync(int orderId)
        {
            var response = await _ordersService.ArgueOrderAsync(orderId);
            return response.Map();
        }

        public async Task<Order> AcceptOrderAsync(int orderId)
        {
            var response = await _ordersService.AcceptOrderAsync(orderId);
            return response.Map();
        }

        public async Task<Order> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var response = await _ordersService.VoteVideoAsync(orderId, isLiked);
            return response.Map();
        }
    }
}