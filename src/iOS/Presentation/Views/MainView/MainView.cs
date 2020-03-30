using System.Linq;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.MainView
{
    [MvxRootPresentation]
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
                SetTabs();
                _tabsInitialized = true;
            }
		}

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        {
            var tabPosition = tabbar.Items.ToList().IndexOf(item);
            ViewModel?.CheckDemoCommand.Execute(tabPosition);
        }

        private void SetTabs()
        {
            TabBar.SetTabBarStyle();
            
            if (TabBar.Items?.Length > 0)
            {
                InitTab(0, "ic_home", Resources.Home_Tab);
                InitTab(1, "ic_competitions", Resources.Competitions_Tab);
                InitCentralTab("ic_create_order");
                InitTab(3, "ic_orders", Resources.Orders_Tab);
                InitTab(4, "ic_profile", Resources.Profile_Tab);
            }
        }

        private void InitTab(int index, string iconName, string title, UIImageRenderingMode mode = UIImageRenderingMode.AlwaysTemplate)
        {
            if (TabBar.Items.Length <= index)
            {
                // TODO: Must have log the error.
                return;
            }

            var tab = TabBar.Items[index];
            tab.Title = title;
            tab.Image = UIImage.FromBundle(iconName).ImageWithRenderingMode(mode);
            tab.SetTabBarItemStyle();
        }

        private void InitCentralTab(string iconName)
        {
            var tab = TabBar.Items[2];
            tab.Image = UIImage.FromBundle(iconName).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            tab.SetCentralTabBarItemStyle();
        }
    }
}