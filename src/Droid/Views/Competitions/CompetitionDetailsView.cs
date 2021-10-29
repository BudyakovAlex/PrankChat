using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.PlatformBusinessServices.Video.Listeners;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.Views.Base;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CompetitionDetailsView : BaseView<CompetitionDetailsViewModel>
    {
        private FrameLayout _loadingOverlay;
        private MvxSwipeRefreshLayout _refreshView;
        private EndlessRecyclerView _recyclerView;

        private SafeLinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;

        private View _uploadingContainerView;
        private CircleProgressBar _uploadingProgressBar;
        private View _uploadingInfoContainer;
        private TextView _uploadedTextView;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_competition_details);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);
            _refreshView = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);

            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.competition_details_recycler_view);
            _layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionDetailsHeaderViewModel, CompetitionDetailsHeaderViewHolder>(Resource.Layout.cell_competition_details_header)
                .AddElement<CompetitionVideoViewModel, CompetitionVideoViewHolder>(Resource.Layout.cell_competition_video);

            var stateScrollListener = new VideoRecyclerViewScrollListener(_layoutManager);
            _recyclerView.AddOnScrollListener(stateScrollListener);

            _uploadingContainerView = FindViewById<View>(Resource.Id.uploading_progress_container);
            _uploadingProgressBar = FindViewById<CircleProgressBar>(Resource.Id.uploading_progress_bar);
            _uploadedTextView = FindViewById<TextView>(Resource.Id.uploaded_text_view);
            _uploadingInfoContainer = FindViewById<View>(Resource.Id.uploading_info_container);
            _uploadingInfoContainer.SetRoundedCorners(DisplayUtils.DpToPx(30) / 2);

            _uploadingProgressBar.ProgressColor = Color.White;
            _uploadingProgressBar.RingThickness = 5;
            _uploadingProgressBar.BaseColor = Color.Gray;
            _uploadingProgressBar.Progress = 0f;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionDetailsView, CompetitionDetailsViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshView).For(v => v.Refreshing).To(vm => vm.IsRefreshing);
            bindingSet.Bind(_refreshView).For(v => v.RefreshCommand).To(vm => vm.RefreshDataCommand);
            bindingSet.Bind(_recyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Bind(_loadingOverlay).For(v => v.Visibility).To(vm => vm.IsBusy)
                      .WithConversion<BoolToGoneConverter>();

            bindingSet.Bind(_uploadingContainerView).For(v => v.BindVisible()).To(vm => vm.IsUploading);
            bindingSet.Bind(_uploadingProgressBar).For(v => v.Progress).To(vm => vm.UploadingProgress);
            bindingSet.Bind(_uploadedTextView).For(v => v.Text).To(vm => vm.UploadingProgressStringPresentation);
            bindingSet.Bind(_uploadingProgressBar).For(v => v.BindClick()).To(vm => vm.CancelUploadingCommand);
        }
    }
}