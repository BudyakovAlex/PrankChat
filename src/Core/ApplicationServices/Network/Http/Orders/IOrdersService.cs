using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders
{
    public interface IOrdersService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderInfo);

        Task<BaseBundleDto<OrderDto>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize);

        Task<BaseBundleDto<OrderDto>> GetUserOwnOrdersAsync(int userId, int page, int pageSize);

        Task<BaseBundleDto<OrderDto>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize);

        Task<OrderDto> GetOrderDetailsAsync(int orderId);

        Task<OrderDto> TakeOrderAsync(int orderId);

        Task<BaseBundleDto<ArbitrationOrderDto>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize);

        Task<OrderDto> CancelOrderAsync(int orderId);

        Task ComplainOrderAsync(int orderId, string title, string description);

        Task<OrderDto> SubscribeOrderAsync(int orderId);

        Task<OrderDto> UnsubscribeOrderAsync(int orderId);

        Task<OrderDto> ArgueOrderAsync(int orderId);

        Task<OrderDto> AcceptOrderAsync(int orderId);

        Task<OrderDto> VoteVideoAsync(int orderId, ArbitrationValueType value);
    }
}