using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;

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
                handleFunc: NavigationService.ShowLoginView);
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

            return NavigationService.ShowUserProfile(_user.Id);
        }
    }
}
