using System.Threading.Tasks;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.BusinessServices.TaskSchedulers;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core
{
    public class CustomAppStart : MvxAppStart
    {
        private readonly INavigationService _navigationService;
        private readonly IBackgroundTaskScheduler _backgroundTaskScheduler;

        public CustomAppStart(IMvxApplication application,
                              IMvxNavigationService mvxNavigationService,
                              INavigationService navigationService,
                              IBackgroundTaskScheduler backgroundTaskScheduler)
            : base(application, mvxNavigationService)
        {
            _navigationService = navigationService;
            _backgroundTaskScheduler = backgroundTaskScheduler;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            _backgroundTaskScheduler.Start();

            if (hint is int orderId)
            {
                return _navigationService.AppStartFromNotification(orderId);
            }

            return _navigationService.AppStart();
        }
    }
}
