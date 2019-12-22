using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        #region Image

        public double DownsampleWidth { get; } = 100;

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        #endregion

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

        public MvxAsyncCommand ShowNotificationCommand
        {
            get { return new MvxAsyncCommand(NavigationService.ShowNotificationView); }
        }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
