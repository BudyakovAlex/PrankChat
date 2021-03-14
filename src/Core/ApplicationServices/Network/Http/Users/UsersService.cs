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
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Providers.UserSession;
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
        private readonly IUserSessionProvider _settingsService;
        private readonly IMvxMessenger _messenger;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public UsersService(
            IUserSessionProvider userSessionProvider,
            IAuthorizationService authorizeService,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger) : base(userSessionProvider, authorizeService, logProvider, messenger)
        {
            _settingsService = userSessionProvider;
            _messenger = messenger;
            _log = logProvider.GetLogFor<UsersService>();

            var configuration = ConfigurationProvider.GetConfiguration();
            _client = new HttpClient(
                configuration.BaseAddress,
                configuration.ApiVersion,
                userSessionProvider,
                _log,
                messenger);

            _messenger.Subscribe<UnauthorizedMessage>(OnUnauthorizedUser, MvxReference.Strong);
        }

        public Task VerifyEmailAsync()
        {
            return _client.PostAsync<ResponseDto>("me/verify/resend");
        }

        public async Task GetCurrentUserAsync()
        {
            try
            {
                if (!Connectivity.NetworkAccess.HasConnection())
                {
                    return;
                }

                var dataApiModel = await _client.GetAsync<ResponseDto<UserDto>>("me", true, IncludeType.Document);
                var user = dataApiModel?.Data; // MappingConfig.Mapper.Map<UserDataModel>(dataApiModel?.Data);
                if (user is null)
                {
                    return;
                }

                _settingsService.User = user.Map();
            }
            catch (Exception ex)
            {
                _log.Warn(ex, ex.Message);
            }
        }

        public async Task<UserDto> GetUserAsync(int userId)
        {
            var dataApiModel = await _client.GetAsync<ResponseDto<UserDto>>($"users/{userId}");
            return dataApiModel?.Data;
        }

        public async Task<UserDto> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            var dataApiModel = await _client.PostAsync<ResponseDto<UserDto>>($"users/{userId}/subscribe", cancellationToken: cancellationToken);
            return dataApiModel?.Data;
        }

        public async Task<UserDto> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            var dataApiModel = await _client.PostAsync<ResponseDto<UserDto>>($"users/{userId}/unsubscribe", cancellationToken: cancellationToken);
            return dataApiModel?.Data;
        }

        public async Task<UserDto> SendAvatarAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<ResponseDto<UserDto>>("me/picture", path, "avatar");
            return dataApiModel?.Data;
        }

        public async Task<UserDto> UpdateProfileAsync(UserUpdateProfileDto userUpdateProfileApiModel)
        {
            var dataApiModel = await _client.PostAsync<UserUpdateProfileDto, ResponseDto<UserDto>>("me", userUpdateProfileApiModel);
            return dataApiModel?.Data;
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            var dataApiModel = new ComplainDto()
            {
                Title = title,
                Description = description
            };
            var url = $"users/{userId}/complaint";
            return _client.PostAsync(url, dataApiModel);
        }

        public async Task<DocumentDto> SendVerifyDocumentAsync(string path)
        {
            var dataApiModel = await _client.PostPhotoFile<ResponseDto<DocumentDto>>("user/dcs", path, "document");
            return dataApiModel?.Data;
        }

        public async Task<CardDto> SaveCardAsync(string number, string userName)
        {
            var createCreditCard = new CreateCardDto()
            {
                Number = number.WithoutSpace(),
                CardUserName = userName,
            };
            var dataApiModel = await _client.PostAsync<CreateCardDto, ResponseDto<CardDto>>("me/cards", createCreditCard, true);
            return dataApiModel?.Data;
        }

        public async Task<CardDto> GetCardsAsync()
        {
            var dataApiModel = await _client.GetAsync<ResponseDto<List<CardDto>>>("me/cards");
            return dataApiModel?.Data?.FirstOrDefault();
        }

        public async Task<BaseBundleDto<UserDto>> GetSubscriptionsAsync(int userId, int page, int pageSize)
        {
            return await _client.GetAsync<BaseBundleDto<UserDto>>($"users/{userId}/subscriptions?page={page}&items_per_page={pageSize}");
        }

        public async Task<BaseBundleDto<UserDto>> GetSubscribersAsync(int userId, int page, int pageSize)
        {
            return await _client.GetAsync<BaseBundleDto<UserDto>>($"users/{userId}/subscribers?page={page}&items_per_page={pageSize}");
        }

        public Task DeleteCardAsync(int id)
        {
            return _client.DeleteAsync($"me/cards/{id}", true);
        }
    }
}