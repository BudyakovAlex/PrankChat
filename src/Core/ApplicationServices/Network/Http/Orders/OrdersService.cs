using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Abstract;
using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Authorization;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Enums;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Orders
{
    public class OrdersService : BaseRestService, IOrdersService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public OrdersService(ISettingsService settingsService,
                             IAuthorizationService authorizeService,
                             IMvxLogProvider logProvider,
                             IMvxMessenger messenger,
                             ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<OrdersService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<OrderApiModel> CreateOrderAsync(CreateOrderApiModel orderInfo)
        {
            var newOrder = await _client.PostAsync<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", orderInfo, true);
            return newOrder?.Data;
        }

        public Task<BaseBundleApiModel<OrderApiModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            return _client.GetAsync<BaseBundleApiModel<OrderApiModel>>($"user/{userId}/orders/own?page={page}&items_per_page={pageSize}",
                                                                             includes: new[] { IncludeType.Customer, IncludeType.Videos });
        }

        public Task<BaseBundleApiModel<OrderApiModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            return _client.GetAsync<BaseBundleApiModel<OrderApiModel>>($"user/{userId}/orders/execute?page={page}&items_per_page={pageSize}",
                                                                             includes: new[] { IncludeType.Customer, IncludeType.Videos });
        }

        public Task<BaseBundleApiModel<OrderApiModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            var endpoint = $"{orderFilterType.GetUrlResource()}?page={page}&items_per_page={pageSize}";
            switch (orderFilterType)
            {
                case OrderFilterType.MyOwn when _settingsService.User != null:
                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;

                case OrderFilterType.MyOwn when _settingsService.User == null:
                case OrderFilterType.MyCompletion when _settingsService.User == null:
                case OrderFilterType.MyOrdered when _settingsService.User == null:
                    return new BaseBundleApiModel<OrderApiModel>();
            }

            return _client.GetAsync<BaseBundleApiModel<OrderApiModel>>(endpoint,
                                                                             includes: new[] { IncludeType.Customer, IncludeType.Videos });
        }

        public async Task<OrderApiModel> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.GetAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return data?.Data;
        }

        public async Task<OrderApiModel> TakeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/executor/appoint", true);
            return data?.Data;
        }

        public async Task<BaseBundleApiModel<ArbitrationOrderApiModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
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
                    if (_settingsService.User == null)
                        return new BaseBundleApiModel<ArbitrationOrderApiModel>();

                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;
            }

            return await _client.GetAsync<BaseBundleApiModel<ArbitrationOrderApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.ArbitrationValues, IncludeType.Customer });
        }

        public async Task<OrderApiModel> CancelOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/cancel", false);
            return data?.Data;
        }

        public Task ComplainOrderAsync(int orderId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"orders/{orderId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<OrderApiModel> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return data?.Data;
        }

        public async Task<OrderApiModel> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return data?.Data;
        }

        public async Task<OrderApiModel> ArgueOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration", true);
            return data?.Data;
        }

        public async Task<OrderApiModel> AcceptOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/finish", true);
            return data?.Data;
        }

        public async Task<OrderApiModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var arbitrationValue = new ChangeArbitrationApiModel()
            {
                Value = isLiked.ToString().ToLower(),
            };
            var data = await _client.PostAsync<ChangeArbitrationApiModel, DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration/value", arbitrationValue, true);
            return data?.Data;
        }
    }
}