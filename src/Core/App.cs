using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<INavigationService, NavigationService>();

            RegisterCustomAppStart<CustomAppStart>();
        }
    }
}
