using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items
{
    public class NotificationItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigationService;

        private NotificationType? _notificationType;
        private int? _userId;

        public NotificationItemViewModel(INavigationService navigationService, NotificationDataModel notificationDataModel)
        {
            _navigationService = navigationService;

            Title = notificationDataModel.Title;
            Description = notificationDataModel.Description;
            DateText = notificationDataModel.CreatedAt?.ToTimeAgoCommentString();

            IsDelivered = notificationDataModel.IsDelivered ?? false;
            _notificationType = notificationDataModel.Type;

            switch (_notificationType)
            {
                case NotificationType.OrderEvent:
                    Title = notificationDataModel.RelatedOrder?.Title;
                    Description = notificationDataModel.RelatedOrder?.Description;
                    break;

                case NotificationType.WalletEvent:
                    Title = notificationDataModel.RelatedTransaction?.Reason;
                    Description = notificationDataModel.RelatedTransaction?.Amount == null ? string.Empty : $"{notificationDataModel.RelatedTransaction?.Amount} ₽";
                    ProfileName = notificationDataModel.RelatedTransaction?.User?.Login;
                    ImageUrl = notificationDataModel.RelatedTransaction?.User?.Avatar;
                    break;

                case NotificationType.SubscriptionEvent:
                case NotificationType.LikeEvent:
                case NotificationType.CommentEvent:
                case NotificationType.ExecutorEvent:
                    ProfileName = notificationDataModel.RelatedUser?.Login;
                    ImageUrl = notificationDataModel.RelatedUser?.Avatar;
                    _userId = notificationDataModel.RelatedUser?.Id;
                    break;
            }

            ShowUserProfileCommand = new MvxAsyncCommand(ShowUserProfileAsync);
        }

        public string ProfileName { get; }

        public string ProfileShortName => ProfileName?.ToShortenName();

        public string ImageUrl { get; }

        public string Description { get; }

        public string DateText { get; }

        public bool IsDelivered { get; }

        public string Title { get; }

        public MvxAsyncCommand ShowUserProfileCommand { get; }

        private Task ShowUserProfileAsync()
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
