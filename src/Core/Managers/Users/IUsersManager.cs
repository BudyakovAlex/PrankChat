using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Models.User;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Managers.Users
{
    public interface IUsersManager
    {
        Task VerifyEmailAsync();

        Task GetAndRefreshUserInSessionAsync();

        Task<Pagination<User>> GetSubscriptionsAsync(int userId, int page, int pageSize);

        Task<Pagination<User>> GetSubscribersAsync(int userId, int page, int pageSize);

        Task<User> UpdateProfileAsync(UserUpdateProfile userInfo);

        Task<User> SendAvatarAsync(string path);

        Task<bool> ComplainUserAsync(int userId, string title, string description);

        Task<User> GetUserAsync(int userId);

        Task<User> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<User> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<Document> SendVerifyDocumentAsync(string path);

        Task<Card> SaveCardAsync(string number, string userName);

        Task<Card> GetCardsAsync();

        Task<bool> SavePasportDataAsync(UserPasportData userPasportData);

        Task DeleteCardAsync(int id);

        Task<object> InviteFriendAsync(string email);
    }
}