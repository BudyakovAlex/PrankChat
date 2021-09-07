using System;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionVideoViewHolder : VideoCardViewHolder<CompetitionVideoViewModel>
    {
        private FrameLayout _videoContainerView;
        private TextView _userNameTextView;
        private TextView _viewsCountTextView;
        private TextView _postDateTextView;
        private CircleCachedImageView _userPhotoImageView;
        private ImageView _likeImageView;
        private TextView _likeTextView;
        private ConstraintLayout _likeButton;

        public CompetitionVideoViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _videoContainerView = view.FindViewById<FrameLayout>(Resource.Id.texture_view_container);
            _userNameTextView = view.FindViewById<TextView>(Resource.Id.user_name_text_view);
            _viewsCountTextView = view.FindViewById<TextView>(Resource.Id.video_info_text_view);
            _postDateTextView = view.FindViewById<TextView>(Resource.Id.post_date_text_view);
            _userPhotoImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.user_photo_image_view);
            _likeImageView = view.FindViewById<ImageView>(Resource.Id.likes_image_view);
            _likeTextView = view.FindViewById<TextView>(Resource.Id.like_text_view);
            _likeButton = view.FindViewById<ConstraintLayout>(Resource.Id.like_button);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<CompetitionVideoViewHolder, CompetitionVideoViewModel>();

            bindingSet.Bind(_userNameTextView).For(v => v.Text).To(vm => vm.UserName);
            bindingSet.Bind(_userPhotoImageView).For(v => v.ImagePath).To(vm => vm.AvatarUrl);
            bindingSet.Bind(_userPhotoImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_userPhotoImageView).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_viewsCountTextView).For(v => v.Text).To(vm => vm.ViewsCount);
            bindingSet.Bind(_postDateTextView).For(v => v.Text).To(vm => vm.PublicationDateString);
            bindingSet.Bind(_likeTextView).For(v => v.Text).To(vm => vm.LikesCount);
            bindingSet.Bind(_likeButton).For(v => v.BindClick()).To(vm => vm.LikeCommand);
            bindingSet.Bind(_likeButton).For(v => v.Selected).To(vm => vm.IsLiked);
            bindingSet.Bind(_likeButton).For(v => v.Visibility).To(vm => vm.CanVoteVideo)
                      .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(StubImageView).For(v => v.ImagePath).To(vm => vm.StubImageUrl);
            bindingSet.Bind(_videoContainerView).For(v => v.BindTouch()).To(vm => vm.ShowFullScreenVideoCommand);
            bindingSet.Bind(_likeTextView).For(v => v.BindTextColor()).To(vm => vm.IsLiked)
                      .WithConversion(BoolToResourceConverter.Name, new Tuple<int, int>(Resource.Color.applicationWhite, Resource.Color.primary_button_border));
            bindingSet.Bind(_likeImageView).For(v => v.BindTintColor()).To(vm => vm.IsLiked)
                      .WithConversion(BoolToResourceConverter.Name, new Tuple<int, int>(Resource.Color.applicationWhite, Resource.Color.primary_button_border));
            bindingSet.Bind(ProcessingView).For(v => v.BindVisible()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(TextureView).For(v => v.BindHidden()).To(vm => vm.IsVideoProcessing);
        }
    }
}