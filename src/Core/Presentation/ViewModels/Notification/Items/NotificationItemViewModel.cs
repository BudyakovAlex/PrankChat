using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items
{
    public class NotificationItemViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private NotificationType? _notificationType;
        private int? _userId;

        public NotificationItemViewModel(INavigationService navigationService,
                                         IUserSessionProvider userSessionProvider,
                                         Models.Data.Notification notification)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            Title = notification.Title;
            Description = notification.Text;
            DateText = notification.CreatedAt?.ToTimeAgoCommentString();

            IsDelivered = notification.IsDelivered ?? false;
            _notificationType = notification.Type;

            switch (_notificationType)
            {
                case NotificationType.WalletEvent:
                    ProfileName = notification.RelatedTransaction?.User?.Login;
                    ImageUrl = notification.RelatedTransaction?.User?.Avatar;
                    break;

                case NotificationType.SubscriptionEvent:
                case NotificationType.LikeEvent:
                case NotificationType.CommentEvent:
                case NotificationType.ExecutorEvent:
                    ProfileName = notification.RelatedUser?.Login;
                    ImageUrl = notification.RelatedUser?.Avatar;
                    _userId = notification.RelatedUser?.Id;
                    break;
            }

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public string ProfileName { get; }

        public string ProfileShortName => ProfileName?.ToShortenName();

        public string ImageUrl { get; }

        public string Description { get; }

        public string DateText { get; }

        private bool _isDelivered;
        public bool IsDelivered
        {
            get => _isDelivered;
            set => SetProperty(ref _isDelivered, value);
        }

        public string Title { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        private Task OpenUserProfileAsync()
        {
            if (_userId is null ||
                _userId.Value == _userSessionProvider.User?.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_userId.Value);
        }
    }
}
