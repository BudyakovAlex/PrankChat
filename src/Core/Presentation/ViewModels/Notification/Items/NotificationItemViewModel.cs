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
        private const int DefaultIdUser = -1;

        private readonly INavigationService _navigationService;

        private NotificationType _typeNotification;
        private int _idUser = DefaultIdUser;

        public string ProfileName { get; }

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ImageUrl { get; }

        public string Description { get; }

        public string DateText { get; }

        public string Status { get; set; }

        public string Title { get; }

        public MvxAsyncCommand OpenProfileUserCommand => new MvxAsyncCommand(OnOpenProfileUser);

        public NotificationItemViewModel(
            INavigationService navigationService,
            string title,
            string description,
            DateTime? notificationDate,
            bool? isDelivered,
            NotificationType? type)
        {
            _navigationService = navigationService;
            Title = title;
            Description = description;
            if (notificationDate != null)
            {
                DateText = ((DateTime)notificationDate).ToTimeAgoCommentString();
            }
            if (isDelivered != null)
            {
                Status = (bool)isDelivered ? Resources.NotificationStatus_Viewed : Resources.NotificationStatus_NotViewed;
            }
            if (type != null)
            {
                _typeNotification = (NotificationType)type;
            }
        }

        public NotificationItemViewModel(
            INavigationService navigationService,
            NotificationMetadataDataModel nmDataModel) : this(navigationService, nmDataModel.Title, nmDataModel.Text, nmDataModel.CreatedAt, nmDataModel.IsDelivered, nmDataModel.Type)
        {
            switch (_typeNotification)
            {
                case NotificationType.OrderEvent:
                    // есть заказ
                    break;

                case NotificationType.WalletEvent:
                    // есть транзакция
                    break;

                case NotificationType.SubscriptionEvent:
                case NotificationType.LikeEvent:
                case NotificationType.CommentEvent:
                case NotificationType.ExecutorEvent:
                    // есть пользователь
                    ProfileName = nmDataModel.RelatedUser.Name;
                    ImageUrl = nmDataModel.RelatedUser.Avatar;
                    _idUser = nmDataModel.RelatedUser.Id;
                    break;

            }
        }

        private Task OnOpenProfileUser()
        {
            if (_idUser == DefaultIdUser)
                return Task.FromResult(0);
            return _navigationService.ShowProfileUser(_idUser);
        }
    }
}
