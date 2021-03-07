using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions.Items
{
    public class SubscriptionItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly User _user;

        public SubscriptionItemViewModel(IUserSessionProvider userSessionProvider, User user)
        {
            _userSessionProvider = userSessionProvider;
            _user = user;

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

        private Task OpenUserProfileAsync()
        {
            if (_user.Id == _userSessionProvider.User.Id ||
                !Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_user.Id);
        }
    }
}
