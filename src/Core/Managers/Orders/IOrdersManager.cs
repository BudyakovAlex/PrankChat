using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Orders
{
    public interface IOrdersManager
    {
        Task<Order> CreateOrderAsync(CreateOrder orderInfo);

        Task<Pagination<Order>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize);

        Task<Pagination<Order>> GetUserOwnOrdersAsync(int userId, int page, int pageSize);

        Task<Pagination<Order>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize);

        Task<Order> GetOrderDetailsAsync(int orderId);

        Task<Order> TakeOrderAsync(int orderId);

        Task<Pagination<ArbitrationOrder>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize);

        Task<Order> CancelOrderAsync(int orderId);

        Task ComplainOrderAsync(int orderId, string title, string description);

        Task<Order> SubscribeOrderAsync(int orderId);

        Task<Order> UnsubscribeOrderAsync(int orderId);

        Task<Order> ArgueOrderAsync(int orderId);

        Task<Order> AcceptOrderAsync(int orderId);

        Task<Order> VoteVideoAsync(int orderId, ArbitrationValueType value);
    }
}