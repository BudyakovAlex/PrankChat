using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
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
    public class CashboxViewModel : BaseViewModel, IMvxViewModel<CashboxTypeNavigationParameter, bool>
    {
        private readonly IMvxMessenger _mvxMessenger;

        private MvxSubscriptionToken _reloadProfileMessageToken;
        private bool _isReloadNeeded;

        public CashboxViewModel(IMediaService mediaService)
        {
            Items = new List<BaseViewModel>
            {
                new RefillViewModel(),
                new WithdrawalViewModel(mediaService)
            };

            ShowContentCommand = new MvxAsyncCommand(NavigationService.ShowCashboxContent);
        }

        public List<BaseViewModel> Items { get; }

        public ICommand ShowContentCommand { get; }

        private int _selectedPage;
        public int SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public override async Task Initialize()
        {
            foreach (var item in Items)
            {
                await item.Initialize();
            }
        }

        public void Prepare(CashboxTypeNavigationParameter parameter)
        {
            switch(parameter.Type)
            {
                case CashboxTypeNavigationParameter.CashboxType.Refill:
                    SelectedPage = Items.IndexOf(Items.SingleOrDefault(item => item is RefillViewModel));
                    break;

                case CashboxTypeNavigationParameter.CashboxType.Withdrawal:
                    SelectedPage = Items.IndexOf(Items.SingleOrDefault(item => item is WithdrawalViewModel));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();

            if (viewFinishing &&
                CloseCompletionSource != null &&
                !CloseCompletionSource.Task.IsCompleted &&
                !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isReloadNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        private void Subscription()
        {
            _reloadProfileMessageToken = _mvxMessenger.SubscribeOnMainThread<ReloadProfileMessage>((msg) => _isReloadNeeded = true);
        }

        private void Unsubscription()
        {
            _reloadProfileMessageToken?.Dispose();
        }
    }
}
