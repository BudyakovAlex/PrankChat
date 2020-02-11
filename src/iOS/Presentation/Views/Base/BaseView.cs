using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseView<TMvxViewModel> : MvxViewController<TMvxViewModel> where TMvxViewModel : BaseViewModel
    {
        private List<UIView> _viewForKeyboardDismiss = new List<UIView>();

        public new string Title { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeKeyboardDismiss();

            SetCommonStyles();
            SetupControls();
            SetCommonControlStyles();
            SetupBinding();
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.BlackOpaque;
        }

        protected bool IsTabbedView { get; set; }

        protected virtual void SetupBinding()
        {
            // nothing do
        }

        protected virtual void SetupControls()
        {
            // nothing do
        }

        protected virtual void SetCommonStyles()
        {
            SetNeedsStatusBarAppearanceUpdate();

            if (NavigationItem.HidesBackButton == false && !IsTabbedView)
            {
                var backButton = NavigationItemHelper.CreateBarButton("ic_back", ViewModel.GoBackCommand);
                NavigationItem.LeftBarButtonItem = backButton;
            }
        }

        protected virtual void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            View.AddGestureRecognizer(new UITapGestureRecognizer(DismissKeyboard));
            foreach (var view in views)
            {
                view.AddGestureRecognizer(new UITapGestureRecognizer(DismissKeyboard));
            }
        }

        protected virtual void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            _viewForKeyboardDismiss = viewList;
            _viewForKeyboardDismiss
                .OfType<UIScrollView>()
                .ToList()
                .ForEach(c => c.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag | UIScrollViewKeyboardDismissMode.Interactive);
        }

        private void SetCommonControlStyles()
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                SetTitle(Title);
            }
        }

        private void SetTitle(string title)
        {
            var titleItem = new UILabel(new CoreGraphics.CGRect(0, 0, 200, 20));
            titleItem.Text = title;
            titleItem.TextAlignment = UITextAlignment.Center;
            titleItem.SetScreenTitleStyle();

            var container = new UIView(new CoreGraphics.CGRect(0, 0, 200, 20));
            container.ContentMode = UIViewContentMode.Center;
            container.AddSubview(titleItem);

            NavigationItem.TitleView = container;
        }

        private void InitializeKeyboardDismiss()
        {
            RegisterKeyboardDismissResponders(new List<UIView> { });
            RegisterKeyboardDismissTextFields(_viewForKeyboardDismiss);
        }

        private void DismissKeyboard()
        {
            foreach (var view in _viewForKeyboardDismiss)
            {
                if (view is UIScrollView)
                    continue;

                view.ResignFirstResponder();
            }
        }
    }
}
