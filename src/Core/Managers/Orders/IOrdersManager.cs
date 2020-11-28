using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Orders
{
    public interface IOrdersManager
    {
        Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo);

        Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize);

        Task<PaginationModel<OrderDataModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize);

        Task<PaginationModel<OrderDataModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize);

        Task<OrderDataModel> GetOrderDetailsAsync(int orderId);

        Task<OrderDataModel> TakeOrderAsync(int orderId);

        Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize);

        Task<OrderDataModel> CancelOrderAsync(int orderId);

        Task ComplainOrderAsync(int orderId, string title, string description);

        Task<OrderDataModel> SubscribeOrderAsync(int orderId);

        Task<OrderDataModel> UnsubscribeOrderAsync(int orderId);

        Task<OrderDataModel> ArgueOrderAsync(int orderId);

        Task<OrderDataModel> AcceptOrderAsync(int orderId);

        Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType value);
    }
}