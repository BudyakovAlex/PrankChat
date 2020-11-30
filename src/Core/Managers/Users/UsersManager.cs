using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Mappers;

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

        public Task GetCurrentUserAsync()
        {
            return _usersService.GetCurrentUserAsync();
        }

        public Task<UserDataModel> GetUserAsync(int userId)
        {
            return _usersService.GetUserAsync(userId);
        }

        public Task<UserDataModel> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            return _usersService.SubscribeToUserAsync(userId, cancellationToken);
        }

        public Task<UserDataModel> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            return _usersService.UnsubscribeFromUserAsync(userId, cancellationToken);
        }

        public Task<UserDataModel> SendAvatarAsync(string path)
        {
            return _usersService.SendAvatarAsync(path);
        }

        public Task<UserDataModel> UpdateProfileAsync(UserUpdateProfileDataModel userInfo)
        {
            return _usersService.UpdateProfileAsync(userInfo);
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            return _usersService.ComplainUserAsync(userId, title, description);
        }

        public Task<DocumentDataModel> SendVerifyDocumentAsync(string path)
        {
            return _usersService.SendVerifyDocumentAsync(path);
        }

        public Task<CardDataModel> SaveCardAsync(string number, string userName)
        {
            return _usersService.SaveCardAsync(number, userName);
        }

        public Task<CardDataModel> GetCardsAsync()
        {
            return _usersService.GetCardsAsync();
        }

        public Task<PaginationModel<UserDataModel>> GetSubscriptionsAsync(int userId, int page, int pageSize)
        {
            return _usersService.GetSubscriptionsAsync(userId, page, pageSize);
        }

        public Task<PaginationModel<UserDataModel>> GetSubscribersAsync(int userId, int page, int pageSize)
        {
            return _usersService.GetSubscribersAsync(userId, page, pageSize);
        }

        public Task DeleteCardAsync(int id)
        {
            return _usersService.DeleteCardAsync(id);
        }
    }
}