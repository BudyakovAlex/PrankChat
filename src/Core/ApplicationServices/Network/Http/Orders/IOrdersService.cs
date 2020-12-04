using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders
{
    public interface IOrdersService
    {
        Task<OrderApiModel> CreateOrderAsync(CreateOrderApiModel orderInfo);

        Task<BaseBundleApiModel<OrderApiModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize);

        Task<BaseBundleApiModel<OrderApiModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize);

        Task<BaseBundleApiModel<OrderApiModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize);

        Task<OrderApiModel> GetOrderDetailsAsync(int orderId);

        Task<OrderApiModel> TakeOrderAsync(int orderId);

        Task<BaseBundleApiModel<ArbitrationOrderApiModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize);

        Task<OrderApiModel> CancelOrderAsync(int orderId);

        Task ComplainOrderAsync(int orderId, string title, string description);

        Task<OrderApiModel> SubscribeOrderAsync(int orderId);

        Task<OrderApiModel> UnsubscribeOrderAsync(int orderId);

        Task<OrderApiModel> ArgueOrderAsync(int orderId);

        Task<OrderApiModel> AcceptOrderAsync(int orderId);

        Task<OrderApiModel> VoteVideoAsync(int orderId, ArbitrationValueType value);
    }
}