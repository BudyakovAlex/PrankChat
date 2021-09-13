using MvvmCross;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.ViewModels.Profile.Cashbox
{
    public class CashboxViewModel : BasePageViewModel<CashboxTypeNavigationParameter, bool>
    {
        private bool _isReloadNeeded;

        public CashboxViewModel()
        {
            Items = new List<BasePageViewModel>
            {
                Mvx.IoCProvider.IoCConstruct<RefillViewModel>(),
                Mvx.IoCProvider.IoCConstruct<WithdrawalViewModel>()
            };

            ShowContentCommand = this.CreateCommand(ShowContentAsync, useIsBusyWrapper: false);
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

        protected override bool DefaultResult => _isReloadNeeded;

        public override void Prepare(CashboxTypeNavigationParameter parameter)
        {
            SelectedPage = parameter.Type switch
            {
                CashboxType.Refill => Items.IndexOf(Items.SingleOrDefault(item => item is RefillViewModel)),
                CashboxType.Withdrawal => Items.IndexOf(Items.SingleOrDefault(item => item is WithdrawalViewModel)),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public override async Task InitializeAsync()
        {
            foreach (var item in Items)
            {
                await item.InitializeAsync();
            }
        }

        private Task ShowContentAsync()
        {
            return Task.WhenAll(
                NavigationManager.NavigateAsync<RefillViewModel>(),
                NavigationManager.NavigateAsync<WithdrawalViewModel>());
        }
    }
}
