using System;
using System.Drawing;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.ProfileView
{
    public partial class CashboxView : BaseViewController<CashboxViewModel>
    {
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetupScrollView();
        }

        public event EventHandler SelectedTabChanged;

        private int _selectedTab;
        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                tabsStackView.SetSelectedTabStyle(value);
            }
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<CashboxView, CashboxViewModel>();

            bindingSet.Bind(this).For(v => v.SelectedTab).To(vm => vm.SelectedPage);
        }

        protected override void SetupControls()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            Title = Resources.Cashbox;

            tabsStackView.SetTabsStyle(new string[] { Resources.Replenishment, Resources.Withdrawal }, OnTabSelected);
        }

        private void HandleScrollViewDecelerationEnded(object sender, EventArgs e)
        {
            var isPageStable = (scrollView.ContentOffset.X % scrollView.Frame.Width) == 0;
            if (isPageStable)
            {
                var calculatedPageIndex = (int)Math.Floor((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
                ViewModel.SelectedPage = calculatedPageIndex;
            }
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedPage))
            {
                SetPageOffset(ViewModel.SelectedPage, true);
            }
        }

        private void OnTabSelected(int position)
        {
            SelectedTab = position;
            SelectedTabChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetPageOffset(int pageIndex, bool animated)
        {
            scrollView.SetContentOffset(new PointF((float)(pageIndex * scrollView.Bounds.Width), 0), animated);
        }

        private void SetupScrollView()
        {
            scrollView.ShowsHorizontalScrollIndicator = false;
            scrollView.ShowsVerticalScrollIndicator = false;
            scrollView.PagingEnabled = true;
            scrollView.DirectionalLockEnabled = true;
            scrollView.ScrollEnabled = true;
            scrollView.Bounces = true;
            scrollView.AlwaysBounceHorizontal = true;
            scrollView.AlwaysBounceVertical = false;
            scrollView.Scrolled += HandleScrollViewDecelerationEnded;

            int i;
            for (i = 0; i < ViewModel.Items.Count; i++)
            {
                var pageViewController = this.CreateViewControllerFor(ViewModel.Items[i]) as UIViewController;
                var view = pageViewController.View;
                view.Frame = new RectangleF((float)scrollView.Bounds.Width * i, 0, (float)scrollView.Bounds.Width, (float)scrollView.Bounds.Height);
                UIView.Transition(scrollView, 0.3, UIViewAnimationOptions.TransitionCrossDissolve, () => scrollView.AddSubview(view), null);
            }

            scrollView.ContentSize = new SizeF((float)scrollView.Bounds.Width * (i == 0 ? 1 : i), (float)scrollView.Bounds.Height);

            SetPageOffset(ViewModel.SelectedPage, false);
        }
    }
}

