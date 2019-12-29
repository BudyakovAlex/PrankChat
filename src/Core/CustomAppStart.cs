using System;
using System.Threading.Tasks;
using MediaManager;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core
{
    public class CustomAppStart : MvxAppStart
    {
        private readonly INavigationService _navigationService;

        public CustomAppStart(
            IMvxApplication application,
            IMvxNavigationService mvxNavigationService,
            INavigationService navigationService)
            : base(application, mvxNavigationService)
        {
            _navigationService = navigationService;

            CrossMediaManager.Current.MediaPlayer.AutoAttachVideoView = false;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return _navigationService.AppStart();
        }
    }
}
