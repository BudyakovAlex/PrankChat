using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseOrderItemViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Order _order;

        private IDisposable _timerTickMessageToken;

        public OrderItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Order order,
            Func<BaseVideoItemViewModel[]> getAllFullScreenVideosFunc) : base(
                videoManager,
                userSessionProvider,
                order.Video,
                order?.Customer.Id,
                getAllFullScreenVideosFunc)
        {
            _userSessionProvider = userSessionProvider;
            _order = order;

            RefreshElapsedTime();

            SystemTimer.SubscribeToEvent(
                OnTimerTick,
                (timer, handler) => timer.TimerElapsed += handler,
                (timer, handler) => timer.TimerElapsed -= handler).DisposeWith(Disposables);

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(
                OpenDetailsOrderAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public int OrderId => _order.Id;

        public string Title => _order.Title;

        public string ProfilePhotoUrl => _order.Customer?.Avatar;

        public string ProfileShortName => _order.Customer?.Login.ToShortenName();

        public OrderType OrderType => _userSessionProvider.User.GetOrderType(_order.Customer?.Id, _order?.Status ?? OrderStatusType.None);

        public OrderTagType OrderTagType => _userSessionProvider.User.GetOrderTagType(_order.Customer?.Id, _order?.Status);

        private TimeSpan? _elapsedTime;
        public TimeSpan? ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                SetProperty(ref _elapsedTime, value);
                RaisePropertiesChanged(nameof(TimeText), nameof(IsTimeAvailable));

                if (!IsTimeAvailable)
                {
                    if (_timerTickMessageToken != null)
                    {
                        Disposables.Remove(_timerTickMessageToken);
                        _timerTickMessageToken.Dispose();
                        _timerTickMessageToken = null;
                    }
                }
            }
        }

        public bool IsTimeAvailable => _elapsedTime.HasValue;

        public string TimeText => _elapsedTime?.ToTimeWithSpaceString();

        public string PriceText => _order.Price.ToPriceString();

        public string StatusText => _order.GetOrderStatusTitle(_userSessionProvider?.User);

        public bool IsHiddenOrder => _order?.OrderCategory == OrderCategory.Private;

        private void OnTimerTick(object _, EventArgs __)
        {
            RefreshElapsedTime();
        }

        private void RefreshElapsedTime()
        {
            ElapsedTime = _order.ActiveTo is null
                ? TimeSpan.FromHours(_order.DurationInHours)
                : _order.GetActiveOrderTime();
        }

        private Task OpenDetailsOrderAsync()
        {
            var items = GetFullScreenVideos();
            var currentItem = items.FirstOrDefault(item => item.VideoId == _order.Video?.Id);
            var index = currentItem is null ? 0 : items.IndexOfOrDefault(currentItem);

            var parameter = new OrderDetailsNavigationParameter(_order.Id, items, index);
            return NavigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter>(parameter);
        }
    }
}