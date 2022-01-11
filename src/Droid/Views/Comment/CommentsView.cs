using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Comment;
using PrankChat.Mobile.Core.ViewModels.Comment.Items;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Comments;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Comment
{
    [MvxActivityPresentation]
    [Activity(WindowSoftInputMode = SoftInput.AdjustResize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class CommentsView : BaseView<CommentsViewModel>
    {
        private MvxSwipeRefreshLayout _refreshView;
        private EndlessRecyclerView _recyclerView;
        private SafeLinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;
        private CircleCachedImageView _profileImageView;
        private EditText _commentEditText;
        private ImageButton _sendCommentImageButton;
        private View _emptyView;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Localization.Resources.Comments;

        private MvxInteraction<int> _scrollInteraction;
        public MvxInteraction<int> ScrollInteraction
        {
            get => _scrollInteraction;
            set
            {
                if (_scrollInteraction != null)
                {
                    _scrollInteraction.Requested -= OnInteractionRequested;
                }

                _scrollInteraction = value;

                if (_scrollInteraction != null)
                {
                    _scrollInteraction.Requested += OnInteractionRequested;
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_comments_view);
        }

        protected override void SetViewProperties()
        {
            InitializeEmptyView();
            _profileImageView = FindViewById<CircleCachedImageView>(Resource.Id.profile_image_view);
            _commentEditText = FindViewById<EditText>(Resource.Id.comment_text_view);
            _sendCommentImageButton = FindViewById<ImageButton>(Resource.Id.create_comment_button);

            _refreshView = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);

            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.recycler_view);
            _layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CommentItemViewModel, CommentViewHolder>(Resource.Layout.cell_comment);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CommentsView, CommentsViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshView).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshView).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);
            bindingSet.Bind(this).For(v => v.ScrollInteraction).To(vm => vm.ScrollInteraction);
            bindingSet.Bind(_recyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_commentEditText).For(v => v.Text).To(vm => vm.Comment);
            bindingSet.Bind(_sendCommentImageButton).For(v => v.BindClick()).To(vm => vm.SendCommentCommand);
            bindingSet.Bind(_emptyView)
              .For(v => v.BindVisible())
              .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
        }

        private void OnInteractionRequested(object sender, MvxValueEventArgs<int> e)
        {
            _recyclerView.SmoothScrollToPosition(e.Value);
        }

        private void InitializeEmptyView()
        {
            _emptyView = FindViewById<View>(Resource.Id.empty_view);
            var emptyViewTitleTextView = _emptyView.FindViewById<TextView>(Resource.Id.title_text_view);
            emptyViewTitleTextView.Text = Core.Localization.Resources.CommentsListIsEmpty;
        }
    }
}