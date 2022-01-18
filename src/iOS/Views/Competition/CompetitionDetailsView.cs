using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class CompetitionDetailsView : BaseViewController<CompetitionDetailsViewModel>
    {
        private MvxUIRefreshControl _refreshControl;
        private VideoTableSource _source;
        private UIBarButtonItem _shareBarItem;

        public bool IsModerationCompleted
        {
            set => NavigationItem.RightBarButtonItem = value ? _shareBarItem : null;
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            _source = new VideoTableSource(tableView);
            _source.Register<CompetitionDetailsHeaderViewModel>(CompetitionDetailsHeaderCell.Nib, CompetitionDetailsHeaderCell.CellId);
            _source.Register<CompetitionVideoViewModel>(CompetitionVideoCell.Nib, CompetitionVideoCell.CellId);

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            tableView.Source = _source;

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;

            uploadingInfoView.Layer.CornerRadius = 15;
            uploadingLabel.SetRegularStyle(12, UIColor.White);

            uploadingProgressBar.ProgressColor = UIColor.White;
            uploadingProgressBar.RingThickness = 5;
            uploadingProgressBar.BaseColor = UIColor.DarkGray;
            uploadingProgressBar.Progress = 0f;

            InitializeNavigationController();
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionDetailsView, CompetitionDetailsViewModel>();

            bindingSet.Bind(_source).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(loadingView).For(v => v.BindVisible()).To(vm => vm.IsBusy);
            bindingSet.Bind(_source).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsRefreshing);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.RefreshDataCommand);
            bindingSet.Bind(uploadingBackgroundView).For(v => v.BindVisible()).To(vm => vm.IsUploading);
            bindingSet.Bind(uploadingProgressBar).For(v => v.Progress).To(vm => vm.UploadingProgress);
            bindingSet.Bind(uploadingLabel).For(v => v.Text).To(vm => vm.UploadingProgressStringPresentation);
            bindingSet.Bind(uploadingProgressBar).For(v => v.BindTap()).To(vm => vm.CancelUploadingCommand);
            bindingSet.Bind(this).For(nameof(IsModerationCompleted)).To(vm => vm.IsModerationCompleted);
        }

        private void InitializeNavigationController()
        {
            _shareBarItem = NavigationItemHelper.CreateBarButton(ImageNames.IconShare, ViewModel.ShareCommand);
            NavigationItem?.SetRightBarButtonItems(new []
            {
               _shareBarItem
            }, true);
        }
    }
}
