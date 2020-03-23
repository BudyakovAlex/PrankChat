using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Configuration;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Api.Base;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;

namespace PrankChat.Mobile.Core.ApplicationServices.Network
{
    public class ApiService : IApiService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;
        private readonly HttpClient _client;

        public ApiService(ISettingsService settingsService,
                          IMvxLogProvider logProvider,
                          IMvxMessenger messenger)
        {
            _settingsService = settingsService;
            _messenger = messenger;

            _log = logProvider.GetLogFor<ApiService>();
            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress, configuration.ApiVersion, settingsService, _log, messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        #region Authorize

        public async Task AuthorizeAsync(string email, string password)
        {
            var loginModel = new AuthorizationApiModel { Email = email, Password = password };
            var authTokenModel = await _client.UnauthorizedPost<AuthorizationApiModel, DataApiModel<AccessTokenApiModel>>("auth/login", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task<bool> AuthorizeExternalAsync(string authToken, LoginType loginType)
        {
            var loginTypePath = GetAuthPathByLoginType(loginType);
            var loginModel = new ExternalAuthorizationApiModel { Token = authToken };
            var authTokenModel = await _client.UnauthorizedPost<ExternalAuthorizationApiModel, DataApiModel<AccessTokenApiModel>>($"auth/social/{loginTypePath}", loginModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
            return authTokenModel?.Data?.AccessToken != null;
        }

        private string GetAuthPathByLoginType(LoginType loginType)
        {
            switch (loginType)
            {
                case LoginType.Vk:
                    return "vk";
                case LoginType.Facebook:
                    return "fb";
                default:
                    throw new ArgumentException();
            }
        }

        public async Task RegisterAsync(UserRegistrationDataModel userInfo)
        {
            var registrationApiModel = MappingConfig.Mapper.Map<UserRegistrationApiModel>(userInfo);
            var authTokenModel = await _client.UnauthorizedPost<UserRegistrationApiModel, DataApiModel<AccessTokenApiModel>>("auth/register", registrationApiModel, true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task LogoutAsync()
        {
            var authTokenModel = await _client.Post<AuthorizationApiModel>("auth/logout", true);
        }

        public async Task RefreshTokenAsync()
        {
            var authTokenModel = await _client.Post<DataApiModel<AccessTokenApiModel>>("auth/refresh", true);
            await _settingsService.SetAccessTokenAsync(authTokenModel?.Data?.AccessToken);
        }

        public async Task<RecoverPasswordResultDataModel> RecoverPasswordAsync(string email)
        {
            var recoverPasswordModel = new RecoverPasswordApiModel { Email = email, };
            var result = await _client.UnauthorizedPost<RecoverPasswordApiModel, RecoverPasswordResultApiModel>("auth/password/email", recoverPasswordModel, false);
            return MappingConfig.Mapper.Map<RecoverPasswordResultDataModel>(result);
        }

        #endregion Authorize

        #region Orders

        public async Task<OrderDataModel> CreateOrderAsync(CreateOrderDataModel orderInfo)
        {
            var createOrderApiModel = MappingConfig.Mapper.Map<CreateOrderApiModel>(orderInfo);
            var newOrder = await _client.Post<CreateOrderApiModel, DataApiModel<OrderApiModel>>("orders", createOrderApiModel);
            return MappingConfig.Mapper.Map<OrderDataModel>(newOrder?.Data);
        }

        public async Task<List<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType)
        {
            string endpoint = "orders";
            switch (orderFilterType)
            {
                case OrderFilterType.New:
                    endpoint = $"{endpoint}?status={OrderStatusType.Active.GetEnumMemberAttrValue()}";
                    break;

                case OrderFilterType.InProgress:
                    if (_settingsService.User == null)
                        return new List<OrderDataModel>();

                    endpoint = $"{endpoint}?executor_id={_settingsService.User.Id}";
                    break;

                case OrderFilterType.MyOwn:
                    if (_settingsService.User == null)
                        return new List<OrderDataModel>();

                    endpoint = $"{endpoint}?customer_id={_settingsService.User.Id}";
                    break;
            }

            var data = await _client.Get<DataApiModel<List<OrderApiModel>>>(endpoint, includes: IncludeType.Customer);
            return MappingConfig.Mapper.Map<List<OrderDataModel>>(data.Data);
        }

        public async Task<OrderDataModel> GetOrderDetailsAsync(int orderId)
        {
            var data = await _client.Get<DataApiModel<OrderApiModel>>($"orders/{orderId}", includes: new IncludeType[] { IncludeType.Customer, IncludeType.Executor, IncludeType.Videos });
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> TakeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/executor/appoint");
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<List<RatingOrderDataModel>> GetRatingOrdersAsync(RatingOrderFilterType filter)
        {
            string endpoint = $"orders?status={OrderStatusType.InArbitration.GetEnumMemberAttrValue()}";
            switch (filter)
            {
                case RatingOrderFilterType.All:
                    // Nothing to do. We should use the 'orders' endpoint to get all rating orders.
                    break;

                case RatingOrderFilterType.New:
                    endpoint = $"{endpoint}&date_from={DateFilterType.Day.GetDateString()}";
                    break;

                case RatingOrderFilterType.My:
                    if (_settingsService.User == null)
                        return new List<RatingOrderDataModel>();

                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;
            }
            var data = await _client.Get<DataApiModel<List<RatingOrderApiModel>>>(endpoint, includes: new IncludeType[] { IncludeType.ArbitrationValues, IncludeType.Customer });
            return MappingConfig.Mapper.Map<List<RatingOrderDataModel>>(data?.Data);
        }

        public async Task<OrderDataModel> CancelOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/cancel", false);
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
            return _client.Post(url, dataApiModel);
        }

        public async Task<OrderDataModel> SubscribeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> UnsubscribeOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/subscribe", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> ArgueOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> AcceptOrderAsync(int orderId)
        {
            var data = await _client.Post<DataApiModel<OrderApiModel>>($"orders/{orderId}/finish", true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        public async Task<OrderDataModel> VoteVideoAsync(int orderId, ArbitrationValueType isLiked)
        {
            var arbitrationValue = new ChangeArbitrationApiModel()
            {
                Value = isLiked.ToString().ToLower(),
            };
            var data = await _client.Post<ChangeArbitrationApiModel, DataApiModel<OrderApiModel>>($"orders/{orderId}/arbitration/value", arbitrationValue, true);
            return MappingConfig.Mapper.Map<OrderDataModel>(data?.Data);
        }

        #endregion Orders

        #region Publications


        public async Task<PaginationModel<VideoDataModel>> GetPopularVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            BaseBundleApiModel<VideoApiModel> videoMetadataBundle;
            if (_settingsService.User == null)
            {
                videoMetadataBundle = await _client.UnauthorizedGet<BaseBundleApiModel<VideoApiModel>>($"videos?popular=true&date_from={dateFilterType.GetDateString()}&page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }
            else
            {
                videoMetadataBundle = await _client.Get<BaseBundleApiModel<VideoApiModel>>($"videos?popular=true&date_from={dateFilterType.GetDateString()}&page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }

            var mappedModels = MappingConfig.Mapper.Map<List<VideoDataModel>>(videoMetadataBundle?.Data);
            var paginationData = videoMetadataBundle.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<VideoDataModel>(mappedModels, totalItemsCount);
        }

        public async Task<PaginationModel<VideoDataModel>> GetActualVideoFeedAsync(DateFilterType dateFilterType, int page, int pageSize)
        {
            BaseBundleApiModel<VideoApiModel> videoMetadataBundle;
            if (_settingsService.User == null)
            {
                videoMetadataBundle = await _client.UnauthorizedGet<BaseBundleApiModel<VideoApiModel>>($"videos?actual=true&date_from={dateFilterType.GetDateString()}&page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }
            else
            {
                videoMetadataBundle = await _client.Get<BaseBundleApiModel<VideoApiModel>>($"videos?actual=true&date_from={dateFilterType.GetDateString()}&page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }

            var mappedModels = MappingConfig.Mapper.Map<List<VideoDataModel>>(videoMetadataBundle?.Data);
            var paginationData = videoMetadataBundle.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<VideoDataModel>(mappedModels, totalItemsCount);
        }

        public async Task<PaginationModel<VideoDataModel>> GetMyVideoFeedAsync(int userId, PublicationType publicationType, int page, int pageSize, DateFilterType? dateFilterType = null)
        {
            if (_settingsService.User == null)
            {
                return new PaginationModel<VideoDataModel>();
            }

            var endpoint = $"orders?page={page}&items_per_page={pageSize}";
            switch (publicationType)
            {
                case PublicationType.MyVideosOfCreatedOrders:
                    endpoint += $"&customer_id={userId}";
                    break;

                case PublicationType.CompletedVideosAssignmentsByMe:
                    endpoint += $"&executor_id={userId}";
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            if (dateFilterType.HasValue)
                endpoint += $"&date_from={dateFilterType.Value.GetDateString()}";

            var dataApiModel = await _client.Get<BaseBundleApiModel<OrderDataModel>>(endpoint, false, IncludeType.Videos, IncludeType.Customer);
            var orderDataModel = MappingConfig.Mapper.Map<List<OrderDataModel>>(dataApiModel?.Data);

            var videoData = orderDataModel?.Where(o => o.Video != null)
                                           .Select(o =>
                                           {
                                               o.Video.User = o.Customer;
                                               return o.Video;
                                           })
                                           .ToList();
       
            var paginationData = dataApiModel.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? videoData.Count;
            return new PaginationModel<VideoDataModel>(videoData, totalItemsCount);
        }

        public async Task<VideoDataModel> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var url = isChecked ? $"videos/{videoId}/like" : $"videos/{videoId}/like/remove";
            var data = await _client.Post<DataApiModel<VideoApiModel>>(url, cancellationToken: cancellationToken);
            return MappingConfig.Mapper.Map<VideoDataModel>(data?.Data);
        }

        #endregion Publications

        #region Users

        public async Task GetCurrentUserAsync()
        {
            var dataApiModel = await _client.Get<DataApiModel<UserApiModel>>("me");
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            _settingsService.User = user;
        }

        public async Task<UserDataModel> SendAvatarAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<UserApiModel>>("me/picture", path);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            return user;
        }

        public async Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo)
        {
            var userUpdateProfileApiModel = MappingConfig.Mapper.Map<UserUpdateProfileApiModel>(userInfo);
            var dataApiModel = await _client.Post<UserUpdateProfileApiModel, DataApiModel<UserApiModel>>("me", userUpdateProfileApiModel);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            return user;
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"users/{userId}/complaint";
            return _client.Post(url, dataApiModel);
        }

        #endregion Users

        #region Video

        public async Task<VideoDataModel> SendVideoAsync(int orderId, string path, string title, string description)
        {
            var loadVideoApiModel = new LoadVideoApiModel()
            {
                OrderId = orderId,
                FilePath = path,
                Title = title,
                Description = description,
            };
            var videoMetadataApiModel = await _client.PostVideoFile<LoadVideoApiModel, DataApiModel<VideoApiModel>>("videos", loadVideoApiModel);
            return MappingConfig.Mapper.Map<VideoDataModel>(videoMetadataApiModel.Data);
        }

        public async Task<long?> RegisterVideoViewedFactAsync(int videoId)
        {
            var videoApiModel = await _client.UnauthorizedGet<DataApiModel<VideoApiModel>>($"videos/{videoId}/looked");
            _log.Log(MvxLogLevel.Debug, () => $"Registered {videoApiModel.Data.ViewsCount} for video with id {videoId}");
            return videoApiModel.Data.ViewsCount;
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"videos/{videoId}/complaint";
            return _client.Post(url, dataApiModel);
        }

        #endregion Video

        #region Payment

        public async Task<PaymentDataModel> RefillAsync(double coast)
        {
            var refillApiData = new RefillApiData
            {
                Amount = coast
            };

            var data = await _client.Post<RefillApiData, DataApiModel<PaymentApiModel>>("payment", refillApiData);
            return MappingConfig.Mapper.Map<PaymentDataModel>(data?.Data);
        }

        public async Task<PaymentDataModel> WithdrawalAsync(double coast)
        {
            return null;
        }

        #endregion Payment

        #region Notification

        public async Task<List<NotificationDataModel>> GetNotificationsAsync()
        {
            var notificationBundle = await _client.Get<BaseBundleApiModel<NotificationApiModel>>("notifications");
            return MappingConfig.Mapper.Map<List<NotificationDataModel>>(notificationBundle?.Data);
        }

        #endregion Notification

        private void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_settingsService.User == null)
                return;

            RefreshTokenAsync().FireAndForget();
        }
    }
}
