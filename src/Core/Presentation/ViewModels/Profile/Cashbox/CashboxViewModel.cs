﻿using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class CashboxViewModel : BasePageViewModel, IMvxViewModel<CashboxTypeNavigationParameter, bool>
    {
        private bool _isReloadNeeded;

        public CashboxViewModel(IMediaService mediaService)
        {
            Items = new List<BasePageViewModel>
            {
                Mvx.IoCProvider.IoCConstruct<RefillViewModel>(),
                Mvx.IoCProvider.IoCConstruct<WithdrawalViewModel>()
            };

            ShowContentCommand = new MvxAsyncCommand(NavigationService.ShowCashboxContent);
            Messenger.SubscribeOnMainThread<ReloadProfileMessage>((msg) => _isReloadNeeded = true).DisposeWith(Disposables);
        }

        public List<BasePageViewModel> Items { get; }

        public ICommand ShowContentCommand { get; }

        private int _selectedPage;
        public int SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public override async Task InitializeAsync()
        {
            foreach (var item in Items)
            {
                await item.InitializeAsync();
            }
        }

        public void Prepare(CashboxTypeNavigationParameter parameter)
        {
            SelectedPage = parameter.Type switch
            {
                CashboxType.Refill => Items.IndexOf(Items.SingleOrDefault(item => item is RefillViewModel)),
                CashboxType.Withdrawal => Items.IndexOf(Items.SingleOrDefault(item => item is WithdrawalViewModel)),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing &&
                CloseCompletionSource != null &&
                !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isReloadNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }
    }
}
