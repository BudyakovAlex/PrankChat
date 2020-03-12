using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items
{
    public class NotificationItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigationService;

        private NotificationType? _notificationType;
        private int? _userId;

        public string ProfileName { get; }

        public string ProfileShortName => ProfileName?.ToShortenName();

        public string ImageUrl { get; }

        public string Description { get; }

        public string DateText { get; }

        public bool IsDelivered { get; }

        public string Title { get; }

        public MvxAsyncCommand ShowUserProfileCommand => new MvxAsyncCommand(OnShowUserProfileAsync);

        public NotificationItemViewModel(INavigationService navigationService,
                                         UserDataModel user,
                                         string title,
                                         string description,
                                         DateTime? createdAt,
                                         bool? isDelivered,
                                         NotificationType? type)
        {
            _navigationService = navigationService;

            Title = title;
            Description = description;
            DateText = createdAt?.ToTimeAgoCommentString();

            IsDelivered = isDelivered ?? false;
            _notificationType = type;

            switch (_notificationType)
            {
                case NotificationType.OrderEvent:
                    // Order created
                    break;

                case NotificationType.WalletEvent:
                    // there are transactions
                    break;

                case NotificationType.SubscriptionEvent:
                case NotificationType.LikeEvent:
                case NotificationType.CommentEvent:
                case NotificationType.ExecutorEvent:
                    ProfileName = user.Name;
                    ImageUrl = user.Avatar;
                    _userId = user.Id;
                    break;

                default:
                    break;
            }
        }

        private Task OnShowUserProfileAsync()
        {
            if (_userId == null)
            {
                // TODO add handler for error.
                return Task.CompletedTask;
            }

            return _navigationService.ShowProfileUser(_userId.Value);
        }
    }
}
