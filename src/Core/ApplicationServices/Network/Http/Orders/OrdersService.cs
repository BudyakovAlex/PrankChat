using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders
{
    public class OrdersService : BaseRestService, IOrdersService
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public OrdersService(
            IUserSessionProvider userSessionProvider,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger) : base(userSessionProvider, authorizeService, logProvider, messenger)
        {
            _userSessionProvider = userSessionProvider;
            _messenger = messenger;
            _log = logProvider.GetLogFor<OrdersService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(
                configuration.BaseAddress,
                configuration.ApiVersion,
                userSessionProvider,
                _log,
                messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderInfo)
        {
            var newOrder = await _client.PostAsync<CreateOrderDto, ResponseDto<OrderDto>>("orders", orderInfo, true);
            return newOrder?.Data;
        }

        public Task<BaseBundleDto<OrderDto>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            return _client.GetAsync<BaseBundleDto<OrderDto>>($"user/{userId}/orders/own?page={page}&items_per_page={pageSize}",
                                                                             includes: new[] { IncludeType.Customer, IncludeType.Videos });
        }

        public Task<BaseBundleDto<OrderDto>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            return _client.GetAsync<BaseBundleDto<OrderDto>>($"user/{userId}/orders/execute?page={page}&items_per_page={pageSize}",
                                                                             includes: new[] { IncludeType.Customer, IncludeType.Videos });
        }

        public Task<BaseBundleDto<OrderDto>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            var endpoint = $"{orderFilterType.GetUrlResource()}?page={page}&items_per_page={pageSize}";
            switch (orderFilterType)
            {
                case OrderFilterType.MyOwn when _userSessionProvider.User != null:
                    endpoint = $"{endpoint}&customer_id={_userSessionProvider.User.Id}";
                    break;

                case OrderFilterType.MyOwn when _userSessionProvider.User == null:
                case OrderFilterType.MyCompletion when _userSessionProvider.User == null:
                case OrderFilterType.MyOrdered when _userSessionProvider.User == null:
                    return Task.FromResult(new BaseBundleDto<OrderDto>());
            }

            return _client.GetAsync<BaseBundleDto<OrderDto>>(endpoint, includes: new[] { IncludeType.Customer, IncludeType.Videos });
        }

        public async Task<OrderDto> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.GetAsync<ResponseDto<OrderDto>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return data?.Data;
        }

        public async Task<OrderDto> TakeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<ResponseDto<OrderDto>>($"orders/{orderId}/executor/appoint", true);
            return data?.Data;
        }

        public async Task<BaseBundleDto<ArbitrationOrderDto>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
        {
            var endpoint = $"orders?page={page}&items_per_page={pageSize}&status={OrderStatusType.InArbitration.GetEnumMemberAttrValue()}";
            switch (filter)
            {
                case ArbitrationOrderFilterType.All:
                    // Nothing to do. We should use the 'orders' endpoint to get all rating orders.
                    break;

                case ArbitrationOrderFilterType.New:
                    endpoint = $"{endpoint}&date_from={DateFilterType.Day.GetDateString()}";
                    break;

                case ArbitrationOrderFilterType.My:
                    if (_userSessionProvider.User == null)
                        return new BaseBundleDto<ArbitrationOrderDto>();

                    endpoint = $"{endpoint}&customer_id={_userSessionProvider.User.Id}";
                    break;
            }

            return await _client.GetAsync<BaseBundleDto<ArbitrationOrderDto>>(endpoint, includes: new IncludeType[] { IncludeType.ArbitrationValues, IncludeType.Customer });
        }

        public async Task<OrderDto> CancelOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<ResponseDto<OrderDto>>($"orders/{orderId}/cancel", false);
            return data?.Data;
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            var dataApiModel = new ComplainDto()
            {
                Title = title,
                Description = description
            };
            var url = $"orders/{orderId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<OrderDto> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<ResponseDto<OrderDto>>($"orders/{orderId}/subscribe", true);
            return data?.Data;
        }

        public async Task<OrderDto> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<ResponseDto<OrderDto>>($"orders/{orderId}/subscribe", true);
            return data?.Data;
        }

        public async Task<OrderDto> ArgueOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<ResponseDto<OrderDto>>($"orders/{orderId}/arbitration", true);
            return data?.Data;
        }

        public async Task<OrderDto> AcceptOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<ResponseDto<OrderDto>>($"orders/{orderId}/finish", true);
            return data?.Data;
        }

        public async Task<OrderDto> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var arbitrationValue = new ChangeArbitrationDto()
            {
                Value = isLiked.ToString().ToLower(),
            };
            var data = await _client.PostAsync<ChangeArbitrationDto, ResponseDto<OrderDto>>($"orders/{orderId}/arbitration/value", arbitrationValue, true);
            return data?.Data;
        }
    }
}