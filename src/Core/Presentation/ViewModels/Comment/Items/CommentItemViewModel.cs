using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items
{
    public class CommentItemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Comment _comment;

        public CommentItemViewModel(INavigationService navigationService,
                                    IUserSessionProvider userSessionProvider,
                                    Models.Data.Comment comment)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            _comment = comment;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
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
