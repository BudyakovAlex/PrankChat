using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items
{
    public class CommentItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Comment _comment;

        public CommentItemViewModel(IUserSessionProvider userSessionProvider, Models.Data.Comment comment)
        {
            _userSessionProvider = userSessionProvider;
            _comment = comment;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public string ProfileName => _comment.User?.Login;

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl => _comment.User?.Avatar;

        public string Comment => _comment.Text;

        public string DateText => _comment.CreatedAt.ToTimeAgoCommentString();

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_comment.User?.Id is null ||
                _comment.User.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.FromResult(false);
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_comment.User.Id);
        }
    }
}
