using System;
using Android.Support.Constraints;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionVideoViewHolder : CardViewHolder
    {
        private TextView _userNameTextView;
        private TextView _viewsCountTextView;
        private TextView _postDateTextView;
        private VideoView _videoView;
        private MvxCachedImageView _stubImageView;
        private CircleCachedImageView _userPhotoImageView;
        private ProgressBar _loadingProgressBar;
        private ImageView _likeImageView;
        private TextView _likeTextView;
        private ConstraintLayout _likeButton;

        public CompetitionVideoViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _userNameTextView = view.FindViewById<TextView>(Resource.Id.user_name_text_view);
            _viewsCountTextView = view.FindViewById<TextView>(Resource.Id.video_info_text_view);
            _postDateTextView = view.FindViewById<TextView>(Resource.Id.post_date_text_view);
            _videoView = view.FindViewById<VideoView>(Resource.Id.video_file);
            _stubImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.stub_image_view);
            _userPhotoImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.user_photo_image_view);
            _loadingProgressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progress_bar);
            _likeImageView = view.FindViewById<ImageView>(Resource.Id.likes_image_view);
            _likeTextView = view.FindViewById<TextView>(Resource.Id.like_text_view);
            _likeButton = view.FindViewById<ConstraintLayout>(Resource.Id.like_button);
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<CompetitionVideoViewHolder, CompetitionVideoViewModel>();

            bindingSet.Bind(_userNameTextView)
                      .For(v => v.Text)
                      .To(vm => vm.UserName);

            bindingSet.Bind(_userPhotoImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.AvatarUrl);

            bindingSet.Bind(_viewsCountTextView)
                      .For(v => v.Text)
                      .To(vm => vm.ViewsCount);

            bindingSet.Bind(_postDateTextView)
                      .For(v => v.Text)
                      .To(vm => vm.PublicationDate);

            bindingSet.Bind(_likeTextView)
                      .For(v => v.Text)
                      .To(vm => vm.LikesCount);

            bindingSet.Bind(_likeButton)
                      .For(v => v.BindClick())
                      .To(vm => vm.LikeCommand);

            bindingSet.Bind(_likeButton)
                      .For(v => v.Visibility)
                      .To(vm => vm.CanVoteVideo)
                      .WithConversion<BoolToGoneConverter>();

            bindingSet.Bind(_stubImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.StubImageUrl);

            bindingSet.Bind(_likeTextView)
                      .For(TextColorTargetBinding.TargetBinding)
                      .To(vm => vm.IsLiked)
                      .WithConversion(BoolToResourceConverter.Name, new Tuple<int, int>(Resource.Color.applicationWhite, Resource.Color.primary_button_border));

            bindingSet.Bind(_likeImageView)
                      .For(ImageViewTintColorTargetBinding.TargetBinding)
                      .To(vm => vm.IsLiked)
                      .WithConversion(BoolToResourceConverter.Name, new Tuple<int, int>(Resource.Color.applicationWhite, Resource.Color.primary_button_border));

            bindingSet.Apply();
        }
    }
}