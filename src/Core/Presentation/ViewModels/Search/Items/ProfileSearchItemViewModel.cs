using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items
{
    public class ProfileSearchItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        private readonly UserDataModel _userDataModel;

        public ProfileSearchItemViewModel(INavigationService navigationService,
                                          ISettingsService settingsService,
                                          UserDataModel userDataModel)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
            _userDataModel = userDataModel;

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public string ProfileName => _userDataModel.Login;

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfileDescription => _userDataModel.Description;

        public string ImageUrl => _userDataModel.Avatar;

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_userDataModel.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_userDataModel.Id);
        }
    }
}