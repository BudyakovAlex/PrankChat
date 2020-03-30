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
            var loginModel = new AuthorizationApiModel { Email = email.ToLower(), Password = password };
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
            var recoverPasswordModel = new RecoverPasswordApiModel { Email = email.ToLower(), };
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

        public async Task<PaginationModel<OrderDataModel>> GetOrdersAsync(OrderFilterType orderFilterType, int page, int pageSize)
        {
            var endpoint = $"orders?page={page}&items_per_page={pageSize}&order_property=created_at";
            switch (orderFilterType)
            {
                case OrderFilterType.All:
                    endpoint = $"{endpoint}";
                    break;

                case OrderFilterType.New:
                    endpoint = $"{endpoint}&status={OrderStatusType.Active.GetEnumMemberAttrValue()}";
                    break;

                case OrderFilterType.InProgress:
                    endpoint = $"{endpoint}&states[]={OrderStatusType.InWork.GetEnumMemberAttrValue()}&states[]={OrderStatusType.WaitFinish.GetEnumMemberAttrValue()}";
                    break;

                case OrderFilterType.MyOwn:
                    if (_settingsService.User == null)
                        return new PaginationModel<OrderDataModel>();

                    endpoint = $"{endpoint}&customer_id={_settingsService.User.Id}";
                    break;

                case OrderFilterType.MyCompleted:
                    if (_settingsService.User == null)
                        return new PaginationModel<OrderDataModel>();

                    endpoint = $"{endpoint}&status={OrderStatusType.Finished.GetEnumMemberAttrValue()}&executor_id={_settingsService.User.Id}";
                    break;
            }

            var data = await _client.Get<BaseBundleApiModel<OrderApiModel>>(endpoint, includes: IncludeType.Customer);
            return CreatePaginationResult<OrderApiModel, OrderDataModel>(data);
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

            return CreatePaginationResult<VideoApiModel, VideoDataModel>(videoMetadataBundle);
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

            var endpoint = string.Empty;
            switch (publicationType)
            {
                case PublicationType.MyVideosOfCreatedOrders:
                    endpoint = $"videos?page={page}&items_per_page={pageSize}&user_id={userId}";
                    break;

                default:
                    throw new InvalidEnumArgumentException();
            }

            if (dateFilterType.HasValue)
                endpoint += $"&date_from={dateFilterType.Value.GetDateString()}";

            var dataApiModel = await _client.Get<BaseBundleApiModel<VideoApiModel>>(endpoint, false, IncludeType.User);
            var orderDataModel = MappingConfig.Mapper.Map<List<VideoDataModel>>(dataApiModel?.Data);
            var paginationData = dataApiModel.Meta.FirstOrDefault();
            var totalItemsCount = paginationData.Value?.Total ?? orderDataModel.Count;
            return new PaginationModel<VideoDataModel>(orderDataModel, totalItemsCount);
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
            var dataApiModel = await _client.Get<DataApiModel<UserApiModel>>("me", includes: IncludeType.Document);
            var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
            _settingsService.User = user;
        }

        public async Task<UserDataModel> SendAvatarAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<UserApiModel>>("me/picture", path, "avatar");
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

        public async Task<DocumentDataModel> SendVerifyDocumentAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<DocumentApiModel>>("user/dcs", path, "document");
            var user = MappingConfig.Mapper.Map<DocumentDataModel>(dataApiModel?.Data);
            return user;
        }

        public async Task<CardDataModel> SaveCardAsync(string number, string userName)
        {
            var createCreditCard = new CreateCardApiModel()
            {
                Number = number.WithoutSpace(),
                CardUserName = userName,
            };
            var dataApiModel = await _client.Post<CreateCardApiModel, DataApiModel<CardApiModel>>("me/cards", createCreditCard, true);
            var user = MappingConfig.Mapper.Map<CardDataModel>(dataApiModel?.Data);
            return user;
        }

        public async Task<CardDataModel> GetCardsAsync()
        {
            var dataApiModel = await _client.Get<DataApiModel<List<CardApiModel>>>("me/cards");
            var data = MappingConfig.Mapper.Map<List<CardDataModel>>(dataApiModel?.Data);
            return data?.FirstOrDefault();
        }

        public Task DeleteCardAsync(int id)
        {
            return _client.Delete($"me/cards/{id}", true);
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

        public async Task<WithdrawalDataModel> WithdrawalAsync(double coast, int cardId)
        {
            var createWithdrawalApiModel = new CreateWithdrawalApiModel()
            {
                Amount = coast,
                CreditCardId = cardId,
            };

            var dataApiModel = await _client.Post<CreateWithdrawalApiModel, DataApiModel<WithdrawalApiModel>>("withdrawal", createWithdrawalApiModel);
            var data = MappingConfig.Mapper.Map<WithdrawalDataModel>(dataApiModel?.Data);
            return data;
        }

        public async Task<List<WithdrawalDataModel>> GetWithdrawalsAsync()
        {
            var dataApiModel = await _client.Get<DataApiModel<List<WithdrawalApiModel>>> ("withdrawal");
            var data = MappingConfig.Mapper.Map<List<WithdrawalDataModel>>(dataApiModel?.Data);
            return data;
        }

        public Task CancelWithdrawalAsync(int withdrawalId)
        {
            return _client.Delete($"withdrawal/{withdrawalId}", true);
        }

        #endregion Payment

        #region Notification

        public async Task<List<NotificationDataModel>> GetNotificationsAsync()
        {
            var notificationBundle = await _client.Get<BaseBundleApiModel<NotificationApiModel>>("notifications");
            return MappingConfig.Mapper.Map<List<NotificationDataModel>>(notificationBundle?.Data);
        }

        #endregion Notification

        #region Competitions

        public async Task<PaginationModel<VideoDataModel>> GetCompetitionVideosAsync(int competitionId, int page, int pageSize)
        {
            BaseBundleApiModel<VideoApiModel> videoMetadataBundle;
            if (_settingsService.User == null)
            {
                videoMetadataBundle = await _client.UnauthorizedGet<BaseBundleApiModel<VideoApiModel>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }
            else
            {
                videoMetadataBundle = await _client.Get<BaseBundleApiModel<VideoApiModel>>($"competition/{competitionId}/videos?page={page}&items_per_page={pageSize}", false, IncludeType.User);
            }

            return CreatePaginationResult<VideoApiModel, VideoDataModel>(videoMetadataBundle);
        }

        public async Task<PaginationModel<CompetitionDataModel>> GetCompetitionsAsync(int page, int pageSize)
        {
            var endpoint = $"competitions?page={page}&items_per_page={pageSize}";
            var data = await _client.Get<BaseBundleApiModel<CompetitionApiModel>>(endpoint);
            return CreatePaginationResult<CompetitionApiModel, CompetitionDataModel>(data);
        }

        #endregion

        private void OnUnauthorizedUser(UnauthorizedMessage obj)
        {
            if (_settingsService.User == null)
                return;

            RefreshTokenAsync().FireAndForget();
        }

        private PaginationModel<TDataModel> CreatePaginationResult<TApiModel, TDataModel>(BaseBundleApiModel<TApiModel> data)
            where TDataModel : class
            where TApiModel : class
        {
            var mappedModels = MappingConfig.Mapper.Map<List<TDataModel>>(data?.Data);
            var paginationData = data.Meta?.FirstOrDefault();
            var totalItemsCount = paginationData?.Value?.Total ?? mappedModels.Count;
            return new PaginationModel<TDataModel>(mappedModels, totalItemsCount);
        }
    }
}
