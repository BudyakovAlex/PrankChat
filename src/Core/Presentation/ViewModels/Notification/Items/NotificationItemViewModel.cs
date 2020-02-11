using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items
{
    public class NotificationItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigationService;

        private DateTime _date;
        private bool _status;
        private string _type;
        private int _idUser;

        public string ProfileName { get; }

        public string ProfileShortName { get; }

        public string ImageUrl { get; }

        public string Description { get; }

        public string DateText => _date.ToTimeAgoCommentString();

        public string Status => _status ? "Просмотрено" : "Непросмотрено";

        public string Title { get; }

        public MvxAsyncCommand OpenProfileUserCommand => new MvxAsyncCommand(OnOpenProfileUser);

        public NotificationItemViewModel(
            INavigationService navigationService,
            string title,
            string description,
            DateTime notificationDate,
            bool status,
            string type)
        {
            _navigationService = navigationService;
            Title = title;
            Description = description;
            _date = notificationDate;
            _status = status;
            _type = type;
        }

        public NotificationItemViewModel(
            INavigationService navigationService,
            NotificationMetadataDataModel nmDataModel) : this(navigationService, nmDataModel.Title, nmDataModel.Text, nmDataModel.CreatedAt, nmDataModel.IsDelivered, nmDataModel.Type)
        {
            switch (_type)
            {
                case "order_event":
                    // есть заказ
                    ProfileName = "Модератор";
                    ProfileShortName = ProfileName.ToShortenName();
                    break;
                case "wallet_event":
                    // есть транзакция
                    break;
                case "subscription_event":
                case "like_event":
                case "comment_event":
                case "executor_event":
                    // есть пользователь
                    ProfileName = nmDataModel.RelatedUser.Name;
                    ProfileShortName = ProfileName.ToShortenName();
                    ImageUrl = nmDataModel.RelatedUser.Avatar;
                    _idUser = nmDataModel.RelatedUser.Id;
                    break;
            }
        }

        private Task OnOpenProfileUser()
        {
            if (_idUser == default(int))
                return Task.FromResult(0);
            return _navigationService.ShowProfileUser(_idUser); ;
        }
    }
}
