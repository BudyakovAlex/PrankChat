using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items
{
    public class ProfileSearchItemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly User _userDataModel;

        public ProfileSearchItemViewModel(INavigationService navigationService,
                                          IUserSessionProvider userSessionProvider,
                                          User userDataModel)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            _userDataModel = userDataModel;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public string ProfileName => _userDataModel.Login;

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfileDescription => _userDataModel.Description;

        public string ImageUrl => _userDataModel.Avatar;

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_userDataModel.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_userDataModel.Id);
        }
    }
}