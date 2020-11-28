using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Network;
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
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Orders
{
    public class OrdersManager : BaseRestService, IOrdersManager
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public OrdersManager(
            ISettingsService settingsService,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger,
            ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<OrdersManager>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            var newOrder = await _client.PostAsync<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", createOrderApiModel, true);
            return MappingConfig.Mapper.Map<OrderDataModel>(newOrder?.Data);
        }

        public async Task<PaginationModel<OrderDataModel>> GetUserOwnOrdersAsync(int userId, int page, int pageSize)
        {
            var data = await _client.GetAsync<BaseBundleApiModel<OrderApiModel>>($"user/{userId}/orders/own?page={page}&items_per_page={pageSize}", includes: new[] { IncludeType.Customer, IncludeType.Videos });
            return CreatePaginationResult<OrderApiModel, OrderDataModel>(data);
        }

        public async Task<PaginationModel<OrderDataModel>> GetUserExecuteOrdersAsync(int userId, int page, int pageSize)
        {
            var data = await _client.GetAsync<BaseBundleApiModel<OrderApiModel>>($"user/{userId}/orders/execute?page={page}&items_per_page={pageSize}", includes: new[] { IncludeType.Customer, IncludeType.Videos });
            return CreatePaginationResult<OrderApiModel, OrderDataModel>(data);
        }

        public async Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
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
                    return new PaginationModel<OrderDataModel>();
            }

            var data = await _client.GetAsync<BaseBundleApiModel<OrderApiModel>>(endpoint, includes: new[] { IncludeType.Customer, IncludeType.Videos });
            return CreatePaginationResult<OrderApiModel, OrderDataModel>(data);
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.GetAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/executor/appoint", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<PaginationModel<ArbitrationOrderDataModel>> GetArbitrationOrdersAsync(ArbitrationOrderFilterType filter, int page, int pageSize)
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
                        return new PaginationModel<ArbitrationOrderDataModel>(new List<ArbitrationOrderDataModel>());

                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;
            }

            var data = await _client.GetAsync<BaseBundleApiModel<ArbitrationOrderApiModel>>(endpoint, includes: new IncludeType[] { IncludeType.ArbitrationValues, IncludeType.Customer });
            return CreatePaginationResult<ArbitrationOrderApiModel, ArbitrationOrderDataModel>(data);
        }

        public async Task<OrderDataModel> CancelOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/cancel", false);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
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

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            var data = await _client.PostAsync<DataApiModel<OrderApiModel>>($"orders/{orderId}/finish", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var arbitrationValue = new ChangeArbitrationApiModel()
            {
                Value = isLiked.ToString().ToLower(),
            };
            var data = await _client.PostAsync<ChangeArbitrationApiModel, DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration/value", arbitrationValue, true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        private PaginationModel<TDataModel> CreatePaginationResult<TApiModel, TDataModel>(BaseBundleApiModel<TApiModel> data)
         where TDataModel : class
         where TApiModel : class
        {
            var mappedModels = MappingConfig.Mapper.Map<List<TDataModel>>(data?.Data ?? new List<TApiModel>());
            var paginationData = data?.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}