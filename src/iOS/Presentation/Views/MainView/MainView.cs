using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.MainView
{
    [MvxRootPresentation(WrapInNavigationController = false)]
    public partial class MainView : MvxTabBarViewController<MainViewModel>
    {
        private bool _tabsInitialized;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!_tabsInitialized)
            {
                ViewModel.ShowContentCommand.Execute();
                _tabsInitialized = true;
            }
        }
    }
}

