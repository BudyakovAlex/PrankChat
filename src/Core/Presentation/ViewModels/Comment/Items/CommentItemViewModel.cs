using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items
{
    public class CommentItemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Comment _commentDataModel;

        public CommentItemViewModel(INavigationService navigationService,
                                    IUserSessionProvider userSessionProvider,
                                    Models.Data.Comment commentDataModel)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            _commentDataModel = commentDataModel;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public string ProfileName => _commentDataModel.User?.Login;

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl => _commentDataModel.User?.Avatar;

        public string Comment => _commentDataModel.Text;

        public string DateText => _commentDataModel.CreatedAt.ToTimeAgoCommentString();

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_commentDataModel.User?.Id is null ||
                _commentDataModel.User.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_commentDataModel.User.Id);
        }
    }
}
