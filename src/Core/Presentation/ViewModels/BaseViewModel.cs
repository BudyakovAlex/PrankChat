using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public INavigationService NavigationService { get; }

        public MvxAsyncCommand GoBackCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.CloseView(this));
            }
        }

        public MvxAsyncCommand ShowSearchCommand
        {
            get { return new MvxAsyncCommand(() => NavigationService.ShowSearchView()); }
        }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
