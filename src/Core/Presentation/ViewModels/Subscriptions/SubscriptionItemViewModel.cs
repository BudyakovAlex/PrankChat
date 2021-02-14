﻿using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions
{
    public class SubscriptionItemViewModel : BasePageViewModel
    {
        private readonly User _userDataModel;

        public SubscriptionItemViewModel(User userDataModel)
        {
            _userDataModel = userDataModel;

            OpenUserProfileCommand = this.CreateRestrictedCommand(OpenUserProfileAsync,
                                                                  restrictedCanExecute: () => SettingsService.User != null,
                                                                  handleFunc: NavigationService.ShowLoginView);
        }

        public string Login => _userDataModel.Login;

        public string ProfileShortLogin => Login.ToShortenName();

        public string Description => _userDataModel.Description;

        public string Avatar => _userDataModel.Avatar;

        public int Id => _userDataModel.Id;

        public ICommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_userDataModel.Id == SettingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return NavigationService.ShowUserProfile(_userDataModel.Id);
        }
    }
}
