using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MvxAsyncCommand ShowContentCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowMainViewContent());
            }
        }

        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
