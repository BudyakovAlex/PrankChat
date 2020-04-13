using System.Threading.Tasks;
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
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            if (hint is int orderId)
            {
                return _navigationService.AppStartFromNotification(orderId);
            }

            return _navigationService.AppStart();
        }
    }
}
