using AndroidX.Core.Content.Resources;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract.Video;
using PrankChat.Mobile.Droid.Bindings;
using PrankChat.Mobile.Droid.Listeners;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Adapters.ViewHolders.Publications
{
    public class PublicationItemViewHolder : VideoCardViewHolder<PublicationItemViewModel>
    {
        private FrameLayout _videoContainerView;
        private View _competitionBorderView;
        private ImageView _competitionCupImageView;
        private CircleCachedImageView _profileImageView;
        private ImageButton _moreImageButton;
        private TextView _profileNameTextView;
        private TextView _videoInfoTextView;
        private TextView _videoNameTextView;
        private ImageButton _shareImageButton;
        private View _likeView;
        private ImageView _likeImageView;
        private TextView _likeTextView;
        private View _dislikeView;
        private ImageView _dislikeImageView;
        private TextView _dislikeTextView;
        private View _commentsView;
        private TextView _comentsTextView;

        private bool _isLiked;
        private bool _isDisliked;

        public PublicationItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        public bool IsLiked
        {
            get => _isLiked;
            set
            {
                _isLiked = value;
                if (_isLiked)
                {
                    _likeImageView.SetImageResource(Resource.Drawable.ic_like);
                    _likeImageView.ImageTintList = ResourcesCompat.GetColorStateList(Context.Resources, Resource.Color.accent, Context.Theme);
                }
                else
                {
                    _likeImageView.SetImageResource(Resource.Drawable.ic_like_hollow);
                    _likeImageView.ImageTintList = null;
                }
            }
        }

        public bool IsDisliked
        {
            get => _isDisliked;
            set
            {
                _isDisliked = value;
                if (_isDisliked)
                {
                    _dislikeImageView.SetImageResource(Resource.Drawable.ic_dislike);
                    _dislikeImageView.ImageTintList = ResourcesCompat.GetColorStateList(Context.Resources, Resource.Color.accent, Context.Theme);
                }
                else
                {
                    _dislikeImageView.SetImageResource(Resource.Drawable.ic_dislike_hollow);
                    _dislikeImageView.ImageTintList = null;
                }
            }
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _videoContainerView = view.FindViewById<FrameLayout>(Resource.Id.texture_view_container);
            _competitionBorderView = view.FindViewById<View>(Resource.Id.competition_border_view);
            _competitionCupImageView = view.FindViewById<ImageView>(Resource.Id.competition_cup_image_view);
            _profileImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_image_view);
            _moreImageButton = view.FindViewById<ImageButton>(Resource.Id.more_image_button);
            _profileNameTextView = view.FindViewById<TextView>(Resource.Id.profile_name_text_view);
            _videoInfoTextView = view.FindViewById<TextView>(Resource.Id.video_info_text_view);
            _videoNameTextView = view.FindViewById<TextView>(Resource.Id.video_name_text_view);
            _shareImageButton = view.FindViewById<ImageButton>(Resource.Id.share_image_button);
            _likeView = view.FindViewById<View>(Resource.Id.like_view);
            _likeImageView = view.FindViewById<ImageView>(Resource.Id.like_image_view);
            _likeTextView = view.FindViewById<TextView>(Resource.Id.like_text_view);
            _dislikeView = view.FindViewById<View>(Resource.Id.dislike_view);
            _dislikeImageView = view.FindViewById<ImageView>(Resource.Id.dislike_image_view);
            _dislikeTextView = view.FindViewById<TextView>(Resource.Id.dislike_text_view);
            _commentsView = view.FindViewById<View>(Resource.Id.comments_view);
            _comentsTextView = view.FindViewById<TextView>(Resource.Id.comments_text_view);

            _moreImageButton.SetOnClickListener(_ => ViewModel.OpenSettingsCommand.Execute());
            _likeView.SetOnClickListener(_ => ViewModel.LikeCommand.Execute());
            _dislikeView.SetOnClickListener(_ => ViewModel.DislikeCommand.Execute());
            _shareImageButton.SetOnClickListener(_ => ViewModel.ShareCommand.Execute());
            _competitionBorderView.SetRoundedCorners(DisplayUtils.DpToPx(23));
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<PublicationItemViewHolder, PublicationItemViewModel>();

            bindingSet.Bind(this).For(v => v.IsLiked).To(vm => vm.IsLiked);
            bindingSet.Bind(this).For(v => v.IsDisliked).To(vm => vm.IsDisliked);
            bindingSet.Bind(_comentsTextView).For(v => v.Text).To(vm => vm.NumberOfCommentsPresentation);
            bindingSet.Bind(_commentsView).For(v => v.BindClick()).To(vm => vm.ShowCommentsCommand);
            bindingSet.Bind(_profileImageView).For(v => v.ImagePath).To(vm => vm.AvatarUrl);
            bindingSet.Bind(_profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_profileImageView).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_profileNameTextView).For(v => v.Text).To(vm => vm.ProfileName);
            bindingSet.Bind(_videoInfoTextView).For(v => v.Text).To(vm => vm.VideoInformationText);
            bindingSet.Bind(_videoNameTextView).For(v => v.Text).To(vm => vm.VideoName);
            bindingSet.Bind(StubImageView).For(v => v.ImagePath).To(vm => vm.StubImageUrl);
            bindingSet.Bind(_likeTextView).For(v => v.Text).To(vm => vm.NumberOfLikesText);
            bindingSet.Bind(_dislikeTextView).For(v => v.Text).To(vm => vm.NumberOfDislikesText);
            bindingSet.Bind(_videoContainerView).For(v => v.BindTouch()).To(vm => vm.ShowFullScreenVideoCommand);
            bindingSet.Bind(ProcessingView).For(v => v.BindVisible()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(TextureView).For(v => v.BindHidden()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(_competitionBorderView).For(v => v.BindVisible()).To(vm => vm.IsCompetitionVideo);
            bindingSet.Bind(_competitionCupImageView).For(v => v.BindVisible()).To(vm => vm.IsCompetitionVideo);
        }
    }
}