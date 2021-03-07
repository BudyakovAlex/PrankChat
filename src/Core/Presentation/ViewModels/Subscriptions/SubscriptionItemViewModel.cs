using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions
{
    public class SubscriptionItemViewModel : BasePageViewModel
    {
        private readonly User _user;

        public SubscriptionItemViewModel(User user)
        {
            _user = user;

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => UserSessionProvider.User != null,
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
            if (_user.Id == UserSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_user.Id);
        }
    }
}
