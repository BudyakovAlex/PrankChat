using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Dtos.Users;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Services.Network.Http.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUserSessionProvider _userSesionProvider;
        private readonly IMvxLog _log;

        private readonly HttpClient _client;

        public UsersService(
            IUserSessionProvider userSessionProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider,
            IMvxLogProvider logProvider,
            IMvxMessenger messenger)
        {
            _userSesionProvider = userSessionProvider;
            _log = logProvider.GetLogFor<UsersService>();

            var environment = environmentConfigurationProvider.Environment;
            _client = new HttpClient(
                environment.ApiUrl,
                environment.ApiVersion,
                userSessionProvider,
                _log,
                messenger);
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

                _userSesionProvider.User = user.Map();
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

        public async Task<bool> ComplainUserAsync(int userId, string title, string description)
        {
            try
            {
                var dataApiModel = new ComplainDto()
                {
                    Title = title,
                    Description = description
                };

                var resource = string.Format(RestConstants.ComplaintUserResourceTemplate, userId);
                await _client.PostAsync(resource, dataApiModel, exceptionThrowingEnabled: true);
                return true;
            }
            catch
            {
                return false;
            }
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

        public async Task<bool> SavePasportDataAsync(UserPasportDataDto userPasportDataDto)
        {
            var dataApiModel = await _client.PostAsync<UserPasportDataDto, ResponseDto<StoredPassportDataDto>>(
                RestConstants.StorePassportDataResource,
                userPasportDataDto,
                true);

            return dataApiModel?.Data != null;
        }
    }
}