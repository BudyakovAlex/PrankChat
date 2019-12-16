using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class CashboxViewModel : BaseViewModel
    {
        private int _selectedPage;

        public CashboxViewModel(INavigationService navigationService) : base(navigationService)
        {
            Items.Add(new RefillViewModel(navigationService));
            Items.Add(new RefillViewModel(navigationService));
        }

        public List<BaseViewModel> Items { get; } = new List<BaseViewModel>();

        public int SelectedPage
        {
            get => _selectedPage;
            set
            {
                if(SetProperty(ref _selectedPage, value))
                {

                }
            }
        }

        public override async Task Initialize()
        {
            foreach (var item in Items)
            {
                await item.Initialize();
            }
        }
    }
}
