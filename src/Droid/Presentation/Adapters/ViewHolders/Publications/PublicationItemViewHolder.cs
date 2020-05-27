using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract.Video;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Converters;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Publications
{
    public class PublicationItemViewHolder : VideoCardViewHolder<PublicationItemViewModel>
    {
        private FrameLayout _videoContainerView;
        private CircleCachedImageView _profileImageView;
        private ImageButton _moreImageButton;
        private TextView _profileNameTextView;
        private TextView _videoInfoTextView;
        private TextView _videoNameTextView;
        private ImageButton _likeImageButton;
        private TextView _likeTextView;
        private ImageButton _shareImageButton;

        public PublicationItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _videoContainerView = view.FindViewById<FrameLayout>(Resource.Id.texture_view_container);
            _profileImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_image_view);
            _moreImageButton = view.FindViewById<ImageButton>(Resource.Id.more_image_button);
            _profileNameTextView = view.FindViewById<TextView>(Resource.Id.profile_name_text_view);
            _videoInfoTextView = view.FindViewById<TextView>(Resource.Id.video_info_text_view);
            _videoNameTextView = view.FindViewById<TextView>(Resource.Id.video_name_text_view);
            _likeImageButton = view.FindViewById<ImageButton>(Resource.Id.like_image_button);
            _likeTextView = view.FindViewById<TextView>(Resource.Id.like_text_view);
            _shareImageButton = view.FindViewById<ImageButton>(Resource.Id.share_image_button);

            _moreImageButton.SetOnClickListener(_ => ViewModel.OpenSettingsCommand.Execute());
            _likeImageButton.SetOnClickListener(_ => ViewModel.LikeCommand.Execute());
            _shareImageButton.SetOnClickListener(_ => ViewModel.ShareCommand.Execute());
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<PublicationItemViewHolder, PublicationItemViewModel>();

            bindingSet.Bind(_profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_profileNameTextView).For(v => v.Text).To(vm => vm.ProfileName);
            bindingSet.Bind(_videoInfoTextView).For(v => v.Text).To(vm => vm.VideoInformationText);
            bindingSet.Bind(_videoNameTextView).For(v => v.Text).To(vm => vm.VideoName);
            bindingSet.Bind(StubImageView).For(v => v.ImagePath).To(vm => vm.VideoPlaceholderImageUrl);
            bindingSet.Bind(_likeImageButton).For(v => v.BindDrawableId()).To(vm => vm.IsLiked)
                      .WithConversion<LikeConverter>();
            bindingSet.Bind(_likeTextView).For(v => v.Text).To(vm => vm.NumberOfLikesText);
            bindingSet.Bind(_videoContainerView).For(ViewTouchTargetBinding.TargetBinding).To(vm => vm.ShowFullScreenVideoCommand);
            bindingSet.Bind(ProcessingView).For(v => v.BindVisible()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(TextureView).For(v => v.BindHidden()).To(vm => vm.IsVideoProcessing);
            bindingSet.Bind(this).For(v => v.CanShowStub).To(vm => vm.IsVideoProcessing)
                      .WithConversion<MvxInvertedBooleanConverter>();

            bindingSet.Apply();
        }
    }
}