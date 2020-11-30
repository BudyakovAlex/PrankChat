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
using PrankChat.Mobile.Core.Models.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users
{
    public class UsersService : BaseRestService, IUsersService
    {
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public UsersService(ISettingsService settingsService,
                            IAuthorizationService authorizeService,
                            IMvxLogProvider logProvider,
                            IMvxMessenger messenger,
                            ILogger logger) : base(settingsService, authorizeService, logProvider, messenger, logger)
        {
            _settingsService = settingsService;
            _messenger = messenger;
            _log = logProvider.GetLogFor<UsersService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(configuration.BaseAddress,
                                     configuration.ApiVersion,
                                     settingsService,
                                     _log,
                                     logger,
                                     messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public Task VerifyEmailAsync()
        {
            return _client.PostAsync<DataApiModel>("me/verify/resend");
        }

        public async Task GetCurrentUserAsync()
        {
            try
            {
                if (!Connectivity.NetworkAccess.HasConnection())
                {
                    return;
                }

                var dataApiModel = await _client.GetAsync<DataApiModel<UserApiModel>>("me", true, IncludeType.Document);
                var user = MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
                if (user is null)
                {
                    return;
                }

                _settingsService.User = user;
            }
            catch (Exception ex)
            {
                _log.Warn(ex, ex.Message);
            }
        }

        public async Task<UserApiModel> GetUserAsync(int userId)
        {
            var dataApiModel = await _client.GetAsync<DataApiModel<UserApiModel>>($"users/{userId}");
            return dataApiModel?.Data;
        }

        public async Task<UserApiModel> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            var dataApiModel = await _client.PostAsync<DataApiModel<UserApiModel>>($"users/{userId}/subscribe", cancellationToken: cancellationToken);
            return dataApiModel?.Data;
        }

        public async Task<UserApiModel> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            var dataApiModel = await _client.PostAsync<DataApiModel<UserApiModel>>($"users/{userId}/unsubscribe", cancellationToken: cancellationToken);
            return dataApiModel?.Data;
        }

        public async Task<UserApiModel> SendAvatarAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<UserApiModel>>("me/picture", path, "avatar");
            return dataApiModel?.Data;
        }

        public async Task<UserApiModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo)
        {
            var userUpdateProfileApiModel = MappingConfig.Mapper.Map<UserUpdateProfileApiModel>(userInfo);
            var dataApiModel = await _client.PostAsync<UserUpdateProfileApiModel, DataApiModel<UserApiModel>>("me", userUpdateProfileApiModel);
            return dataApiModel?.Data;
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            var dataApiModel = new ComplainApiModel()
            {
                Title = title,
                Description = description
            };
            var url = $"users/{userId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<DocumentApiModel> SendVerifyDocumentAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<DataApiModel<DocumentApiModel>>("user/dcs", path, "document");
            return dataApiModel?.Data;
        }

        public async Task<CardApiModel> SaveCardAsync(string number, string userName)
        {
            var createCreditCard = new CreateCardApiModel()
            {
                Number = number.WithoutSpace(),
                CardUserName = userName,
            };
            var dataApiModel = await _client.PostAsync<CreateCardApiModel, DataApiModel<CardApiModel>>("me/cards", createCreditCard, true);
            return dataApiModel?.Data;
        }

        public async Task<CardApiModel> GetCardsAsync()
        {
            var dataApiModel = await _client.GetAsync<DataApiModel<List<CardApiModel>>>("me/cards");
            return dataApiModel?.Data?.FirstOrDefault();
        }

        public async Task<BaseBundleApiModel<UserApiModel>> GetSubscriptionsAsync(int userId, int page, int pageSize)
        {
            return await _client.GetAsync<BaseBundleApiModel<UserApiModel>>($"users/{userId}/subscriptions?page={page}&items_per_page={pageSize}");
        }

        public async Task<BaseBundleApiModel<UserApiModel>> GetSubscribersAsync(int userId, int page, int pageSize)
        {
            return await _client.GetAsync<BaseBundleApiModel<UserApiModel>>($"users/{userId}/subscribers?page={page}&items_per_page={pageSize}");
        }

        public Task DeleteCardAsync(int id)
        {
            return _client.DeleteAsync($"me/cards/{id}", true);
        }

    }
}