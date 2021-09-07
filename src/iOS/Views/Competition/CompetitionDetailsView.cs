using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    public partial class CompetitionDetailsView : BaseView<CompetitionDetailsViewModel>
    {
        private MvxUIRefreshControl _refreshControl;
        private VideoTableSource _source;

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
        }
    }
}
