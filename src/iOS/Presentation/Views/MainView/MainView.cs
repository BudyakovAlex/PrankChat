using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.MainView
{
    [MvxRootPresentation]
    public partial class MainView : MvxTabBarViewController<MainViewModel>
    {
        private bool _tabsInitialized;
        private int _lastTabPosition;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (_tabsInitialized)
            {
                return;
            }

            ViewModel.LoadContentCommand.Execute(null);
            SetTabs();
            _tabsInitialized = true;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        {
            var tabPosition = tabbar.Items.ToList().IndexOf(item);
            if (tabPosition == _lastTabPosition)
            {
                ItemReselected(tabPosition);
            }
            else
            {
                if (tabPosition != 0 && tabPosition != 3)
                {
                    RefreshTabData(tabPosition);
                }
            }

            _lastTabPosition = tabPosition;
            ViewModel?.CheckDemoCommand.Execute(tabPosition);

            if (ViewModel?.CanSwitchTabs(tabPosition) ?? false)
            {
                ViewModel?.ShowWalkthrouthIfNeedCommand?.Execute(tabPosition);
            }
        }

        private void ItemReselected(int position) =>
            RefreshTabData(position);

        private void RefreshTabData(int position)
        {
            var viewController = ViewControllers.ElementAtOrDefault(position);
            switch (viewController)
            {
                case null:
                    return;

                case UINavigationController navigationController:
                    ScrollContentToTop(navigationController.VisibleViewController);
                    _ = RefreshContentAsync(navigationController.VisibleViewController);
                    break;

                default:
                    ScrollContentToTop(viewController);
                    _ = RefreshContentAsync(viewController);
                    break;
            }
        }

        private void ScrollContentToTop(UIViewController viewController)
        {
            if (viewController is IScrollableView scrollableView && scrollableView.ScrollView != null)
            {
                scrollableView.ScrollView.SetContentOffset(new CoreGraphics.CGPoint(0, 0), false);
            }
        }

        private async Task RefreshContentAsync(UIViewController viewController)
        {
            if (viewController is IRefreshableView refreshableView)
            {
                //NOTE: need to smooth scroll offset on top before refreshing data
                await Task.Delay(400);
                refreshableView.RefreshData();
            }
        }

        private void SetTabs()
        {
            TabBar.SetTabBarStyle();
            
            if (TabBar.Items?.Length > 0)
            {
                InitTab(0, ImageNames.IconHome, Resources.Home);
                InitTab(1, ImageNames.IconCompetitions, Resources.Contests);
                InitCentralTab(ImageNames.IconCreateOrder);
                InitTab(3, ImageNames.IconOrders, Resources.Orders);
                InitTab(4, ImageNames.IconProfile, Resources.Profile);
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
            tab.Image = UIImage.FromBundle(iconName)
                               .ImageWithRenderingMode(mode);
            tab.SetTabBarItemStyle();
        }

        private void InitCentralTab(string iconName)
        {
            var tab = TabBar.Items[2];
            tab.Image = UIImage.FromBundle(iconName)
                               .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            tab.SetCentralTabBarItemStyle();
        }
    }
}