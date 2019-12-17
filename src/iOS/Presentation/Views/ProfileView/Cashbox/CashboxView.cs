using System;
using System.Drawing;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    public partial class CashboxView : BaseGradientBarView<CashboxViewModel>
    {
        private int _page;

        public int Page
        {
            get => _page;
            set => scrollView.SetContentOffset(new PointF((float)(value * scrollView.Bounds.Width), 0), true);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            SetupScrollView();
        }

        protected override void SetupBinding()
        {
            var set = this.CreateBindingSet<CashboxView, CashboxViewModel>();

            set.Bind(segmentedControl)
                .To(vm => vm.SelectedPage);

            set.Apply();
        }

        protected override void SetupControls()
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            Title = Resources.CreateOrderView_Title;
            segmentedControl.SetStyle(new[] { Resources.CashboxView_Fillup_Tab, Resources.CashboxView_Withdrawal_Tab });
        }

        private void HandleScrollViewDecelerationEnded(object sender, EventArgs e)
        {
            var pageScrolledTo = (int)Math.Floor((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
            ViewModel.SelectedPage = pageScrolledTo;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedPage))
            {
                Page = ViewModel.SelectedPage;
            }
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
                scrollView.AddSubview(view);
            }

            scrollView.ContentSize = new SizeF((float)scrollView.Bounds.Width * (i == 0 ? 1 : i), (float)scrollView.Bounds.Height);
        }
    }
}

