﻿using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
	public class OrderItemViewModel : BaseItemViewModel, IDisposable
	{
		private readonly INavigationService _navigationService;
		private readonly ISettingsService _settingsService;
		private readonly IMvxMessenger _mvxMessenger;

		private DateTime? _activeTo;
		private OrderStatusType _status;
		private int? _customerId;
		private MvxSubscriptionToken _timerTickMessageToken;

		public int OrderId { get; }

		public string Title { get; }

		public string ProfilePhotoUrl { get; }

		public string ProfileShortName { get; }

		public OrderType OrderType => _settingsService.User.GetOrderType(_customerId, _status);

		private TimeSpan? _elapsedTime;
		public TimeSpan? ElapsedTime
		{
			get => _elapsedTime;
			set
			{
				if (SetProperty(ref _elapsedTime, value))
				{
					RaisePropertyChanged(nameof(TimeText));
				}

                if (!IsTimeAvailable)
                {
					Unsubscribe();
				}
			}
		}

		public bool IsTimeAvailable => _elapsedTime > new TimeSpan();

		public string TimeText => _elapsedTime?.ToTimeWithSpaceString();

		public string PriceText { get; }

		public string StatusText
		{
			get
			{
				switch (_status)
				{
					case OrderStatusType.New:
						return _settingsService.User?.Id == _customerId ? Resources.OrderStatus_MyOrder : Resources.OrderStatus_New;

					case OrderStatusType.Rejected:
						return Resources.OrderStatus_Rejected;

					case OrderStatusType.Cancelled:
						return Resources.OrderStatus_Cancelled;

					case OrderStatusType.Active:
						return Resources.OrderStatus_Active;

					case OrderStatusType.InWork:
						return Resources.OrderStatus_InWork;

					case OrderStatusType.InArbitration:
						return Resources.OrderStatus_InArbitration;

					case OrderStatusType.ProcessCloseArbitration:
						return Resources.OrderStatus_ProcessCloseArbitration;

					case OrderStatusType.ClosedAfterArbitrationCustomerWin:
						return Resources.OrderStatus_ClosedAfterArbitrationCustomerWin;

					case OrderStatusType.ClosedAfterArbitrationExecutorWin:
						return Resources.OrderStatus_ClosedAfterArbitrationExecutorWin;

					case OrderStatusType.WaitFinish:
						return Resources.OrderStatus_WaitFinish;

					case OrderStatusType.Finished:
						return Resources.OrderStatus_Finished;

					default:
						return string.Empty;
				}
			}
		}

		public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

		public OrderItemViewModel(INavigationService navigationService,
								  ISettingsService settingsService,
								  IMvxMessenger mvxMessenger,
								  int orderId,
								  string orderTitle,
								  string profilePhotoUrl,
								  string profileName,
								  double? price,
								  DateTime? activeTo,
								  OrderStatusType status,
								  int? customerId)
		{
			_navigationService = navigationService;
			_settingsService = settingsService;
			_mvxMessenger = mvxMessenger;

			Title = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = price.ToPriceString();
			ProfileShortName = profileName.ToShortenName();
			_activeTo = activeTo;
			_status = status;
			OrderId = orderId;
			_customerId = customerId;

			Subscribe();
			OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Unsubscribe();
			}
		}

		private void Subscribe()
		{
			_timerTickMessageToken = _mvxMessenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong);
		}

		private void Unsubscribe()
		{
			if (_timerTickMessageToken != null)
			{
				_mvxMessenger.Unsubscribe<TimerTickMessage>(_timerTickMessageToken);
				_timerTickMessageToken = null;
			}
		}

		private void OnTimerTick(TimerTickMessage message)
		{
			ElapsedTime = _activeTo?.ToLocalTime() - DateTime.Now;
		}

		private async Task OnOpenDetailsOrderAsync()
		{
			var result = await _navigationService.ShowOrderDetailsView(OrderId);
			if (result == null)
				return;

			_status = result.Status ?? OrderStatusType.None;
			if (_status == OrderStatusType.Cancelled || _status == OrderStatusType.Finished)
			{
				_mvxMessenger.Publish(new RemoveOrderMessage(this, OrderId));
				return;
			}

			await RaisePropertyChanged(nameof(StatusText));
			await RaisePropertyChanged(nameof(OrderType));
		}
	}
}
