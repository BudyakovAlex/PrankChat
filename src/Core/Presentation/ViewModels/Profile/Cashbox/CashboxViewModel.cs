using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class CashboxViewModel : BaseViewModel, IMvxViewModel<CashboxTypeNavigationParameter>
    {
        private int _selectedPage;

        public CashboxViewModel(INavigationService navigationService) : base(navigationService)
        {
            Items.Add(new RefillViewModel(navigationService));
            Items.Add(new WithdrawalViewModel(navigationService));
        }

        public List<BaseViewModel> Items { get; } = new List<BaseViewModel>();

        public int SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
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
