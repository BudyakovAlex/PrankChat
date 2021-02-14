using PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Data.Dtos;
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

        public Task GetCurrentUserAsync()
        {
            return _usersService.GetCurrentUserAsync();
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var response = await _usersService.GetUserAsync(userId);
            return response.Map();
        }

        public async Task<User> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            var response = await _usersService.SubscribeToUserAsync(userId, cancellationToken);
            return response.Map();
        }

        public async Task<User> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null)
        {
            var response = await _usersService.UnsubscribeFromUserAsync(userId, cancellationToken);
            return response.Map();
        }

        public async Task<User> SendAvatarAsync(string path)
        {
            var response = await _usersService.SendAvatarAsync(path);
            return response.Map();
        }

        public async Task<User> UpdateProfileAsync(UserUpdateProfile userInfo)
        {
            var apiModel = new UserUpdateProfileDto()
            {
                Name = userInfo.Name,
                Email = userInfo.Email,
                Login = userInfo.Login,
                Sex = userInfo.Sex,
                Birthday = userInfo.Birthday,
                Description = userInfo.Description,
            };

            var response = await _usersService.UpdateProfileAsync(apiModel);
            return response.Map();
        }

        public Task ComplainUserAsync(int userId, string title, string description)
        {
            return _usersService.ComplainUserAsync(userId, title, description);
        }

        public async Task<Document> SendVerifyDocumentAsync(string path)
        {
            var response = await _usersService.SendVerifyDocumentAsync(path);
            return response.Map();
        }

        public async Task<Card> SaveCardAsync(string number, string userName)
        {
            var response = await _usersService.SaveCardAsync(number, userName);
            return response.Map();
        }

        public async Task<Card> GetCardsAsync()
        {
            var response = await _usersService.GetCardsAsync();
            return response.Map();
        }

        public async Task<Pagination<User>> GetSubscriptionsAsync(int userId, int page, int pageSize)
        {
            var response = await _usersService.GetSubscriptionsAsync(userId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Pagination<User>> GetSubscribersAsync(int userId, int page, int pageSize)
        {
            var response = await _usersService.GetSubscribersAsync(userId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public Task DeleteCardAsync(int id)
        {
            return _usersService.DeleteCardAsync(id);
        }
    }
}