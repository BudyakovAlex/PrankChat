using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxViewController<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        private List<UIView> _viewForKeyboardDismiss = new List<UIView>();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeKeyboardDismiss();

            SetCommonStyles();
            SetupControls();
            SetupBinding();
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.BlackOpaque;
        }

        protected abstract void SetupBinding();

        protected abstract void SetupControls();

        protected virtual void SetCommonStyles()
        {
            SetNeedsStatusBarAppearanceUpdate();

            if (ParentViewController != null)
            {
                var backButton = NavigationItemHelper.CreateBarButton("ic_back", ViewModel.GoBackCommand);
                NavigationItem.LeftBarButtonItem = backButton;
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                SetTitle(Title);
            }
        }

        protected virtual void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            foreach(var view in views)
            {
                var tapGesture = new UITapGestureRecognizer(DismissKeyboard);
                view.AddGestureRecognizer(tapGesture);
            }
        }

        protected virtual void RegisterKeyboardDismissViews(List<UIView> viewList)
        {
            _viewForKeyboardDismiss = viewList;
            _viewForKeyboardDismiss
                .OfType<UIScrollView>()
                .ToList()
                .ForEach(c => c.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag | UIScrollViewKeyboardDismissMode.Interactive);
        }

        private void SetTitle(string title)
        {
            var titleItem = new UILabel(new CoreGraphics.CGRect(0,0, 20, 100));
            titleItem.Text = title;
            titleItem.SetTitleStyle();

            var container = new UIView(new CoreGraphics.CGRect(0, 0, 20, 100));
            container.AddSubview(titleItem);

            NavigationController.NavigationItem.TitleView = container;
        }

        private void InitializeKeyboardDismiss()
        {
            RegisterKeyboardDismissResponders(new List<UIView> { View });
            RegisterKeyboardDismissViews(_viewForKeyboardDismiss);
        }

        private void DismissKeyboard()
        {
            foreach(var view in _viewForKeyboardDismiss)
            {
                if (view is UIScrollView)
                    continue;

                view.ResignFirstResponder();
            }
        }
    }
}
