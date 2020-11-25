using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items
{
    public class CommentItemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        private readonly CommentDataModel _commentDataModel;

        public CommentItemViewModel(INavigationService navigationService,
                                    ISettingsService settingsService,
                                    CommentDataModel commentDataModel)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
            _commentDataModel = commentDataModel;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
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
                _commentDataModel.User.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_commentDataModel.User.Id);
        }
    }
}
