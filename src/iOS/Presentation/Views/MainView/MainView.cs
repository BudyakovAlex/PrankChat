using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.MainView
{
    [MvxRootPresentation(WrapInNavigationController = false)]
    public partial class MainView : MvxTabBarViewController<MainViewModel>
    {
        private bool _tabsInitialized;

        public UIBarButtonItem NotificationBarButton { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupControls();
            SetupBinding();
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

        private void SetupBinding()
        {
            var set = this.CreateBindingSet<MainView, MainViewModel>();
            //set.Bind(this).For(v => v.NotificationBarButton).To(vm => vm.ShowNotificationCommand);
            set.Apply();
        }

        private void SetupControls()
        {
            NotificationBarButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, null);
            //NotificationBarButton = new UIBarButtonItem("Notification", UIBarButtonItemStyle.Plain, null);
            NavigationItem.RightBarButtonItem = NotificationBarButton;
        }
    }
}

