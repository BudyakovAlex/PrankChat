using PrankChat.Mobile.Core.Data.Dtos;
using PrankChat.Mobile.Core.Data.Dtos.Base;
using PrankChat.Mobile.Core.Data.Dtos.Users;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Network.Http.Users
{
    public interface IUsersService
    {
        Task VerifyEmailAsync();

        Task GetCurrentUserAsync();

        Task<BaseBundleDto<UserDto>> GetSubscriptionsAsync(int userId, int page, int pageSize);

        Task<BaseBundleDto<UserDto>> GetSubscribersAsync(int userId, int page, int pageSize);

        Task<UserDto> UpdateProfileAsync(UserUpdateProfileDto userInfo);

        Task<UserDto> SendAvatarAsync(string path);

        Task ComplainUserAsync(int userId, string title, string description);

        Task<UserDto> GetUserAsync(int userId);

        Task<UserDto> SubscribeToUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<UserDto> UnsubscribeFromUserAsync(int userId, CancellationToken? cancellationToken = null);

        Task<DocumentDto> SendVerifyDocumentAsync(string path);

        Task<CardDto> SaveCardAsync(string number, string userName);

        Task<bool> SavePasportDataAsync(UserPasportDataDto userPasportDataDto);

        Task<CardDto> GetCardsAsync();

        Task DeleteCardAsync(int id);
    }
}