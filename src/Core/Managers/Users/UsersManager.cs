using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Users
{
    public class UsersManager : IUsersManager
    {
        private readonly IUsersService _usersService;

        public UsersManager(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public Task VerifyEmailAsync()
        {
            return _usersService.VerifyEmailAsync();
        }

        public async Task GetCurrentUserAsync()
        {
            await _usersService.GetCurrentUserAsync();
        }

        public async Task<UserDataModel> GetUserAsync(int userId)
        {
            return await _usersService.GetUserAsync(userId);
        }

        public async Task<UserDataModel> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            return await _usersService.SubscribeToUserAsync(userId, cancellationToken);
        }

        public async Task<UserDataModel> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            return await _usersService.UnsubscribeFromUserAsync(userId, cancellationToken);
        }

        public async Task<UserDataModel> SendAvatarAsync(string path)
        {
            return await _usersService.SendAvatarAsync(path);
        }

        public async Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo)
        {
            return await _usersService.UpdateProfileAsync(userInfo);
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            return _usersService.ComplainUserAsync(userId, title, description);
        }

        public async Task<DocumentDataModel> SendVerifyDocumentAsync(string path)
        {
            return await _usersService.SendVerifyDocumentAsync(path);
        }

        public async Task<CardDataModel> SaveCardAsync(string number, string userName)
        {
            return await _usersService.SaveCardAsync(number, userName);
        }

        public async Task<CardDataModel> GetCardsAsync()
        {
            return await _usersService.GetCardsAsync();
        }

        public async Task<PaginationModel<UserDataModel>> GetSubscriptionsAsync(int userId, int page, int pageSize)
        {
            return await _usersService.GetSubscriptionsAsync(userId, page, pageSize);
        }

        public async Task<PaginationModel<UserDataModel>> GetSubscribersAsync(int userId, int page, int pageSize)
        {
            return await _usersService.GetSubscribersAsync(userId, page, pageSize);
        }

        public Task DeleteCardAsync(int id)
        {
            return _usersService.DeleteCardAsync(id);
        }
    }
}