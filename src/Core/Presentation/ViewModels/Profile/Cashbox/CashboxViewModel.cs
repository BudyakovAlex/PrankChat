using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox
{
    public class CashboxViewModel : BaseViewModel, IMvxViewModel<CashboxTypeNavigationParameter>
    {
        public List<BaseViewModel> Items { get; } = new List<BaseViewModel>();

        public ICommand ShowContentCommand => new MvxAsyncCommand(NavigationService.ShowCashboxContent);

        private int _selectedPage;
        public int SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
        }

        public CashboxViewModel(INavigationService navigationService,
                        IErrorHandleService errorHandleService,
                        IApiService apiService,
                        IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            Items.Add(new RefillViewModel(navigationService, errorHandleService, apiService, dialogService));
            Items.Add(new WithdrawalViewModel(navigationService, errorHandleService, apiService, dialogService));
        }

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
                    SelectedPage = Items.IndexOf(Items.SingleOrDefault(c => c is RefillViewModel));
                    break;

                case CashboxTypeNavigationParameter.CashboxType.Withdrawal:
                    SelectedPage = Items.IndexOf(Items.SingleOrDefault(c => c is WithdrawalViewModel));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
