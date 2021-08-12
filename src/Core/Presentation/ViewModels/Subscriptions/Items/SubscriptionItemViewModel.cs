using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions.Items
{
    public class SubscriptionItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Func<Task> _refreshDataFunc;
        private readonly User _user;

        public SubscriptionItemViewModel(IUserSessionProvider userSessionProvider, User user, Func<Task> refreshDataFunc)
        {
            _userSessionProvider = userSessionProvider;
            _user = user;
            _refreshDataFunc = refreshDataFunc;

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public string Login => _user.Login;

        public string ProfileShortLogin => Login.ToShortenName();

        public string Description => _user.Description;

        public string Avatar => _user.Avatar;

        public int Id => _user.Id;

        public ICommand OpenUserProfileCommand { get; }

        private async Task OpenUserProfileAsync()
        {
            if (_user.Id == _userSessionProvider.User.Id ||
                !Connectivity.NetworkAccess.HasConnection())
            {
                return;
            }

            var shouldRefresh = await NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_user.Id);
            if (!shouldRefresh)
            {
                return;
            }

            var refreshTask = _refreshDataFunc?.Invoke() ?? Task.CompletedTask;
            await refreshTask;
        }
    }
}
