using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public partial class PublicationItemCell : BaseVideoTableCell<PublicationItemCell, PublicationItemViewModel>
	{
		protected PublicationItemCell(IntPtr handle)
            : base(handle)
		{
		}

		protected override UIView VideoView => videoView;

		protected override UIActivityIndicatorView LoadingActivityIndicator => loadingActivityIndicator;

		protected override UIImageView StubImageView => stubImageView;

		protected override void SetupControls()
		{
			base.SetupControls();

			videoView.SetPreviewStyle();
			profileNameLabel.SetMainTitleStyle();
			publicationInfoLabel.SetSmallSubtitleStyle();
			videoNameLabel.SetTitleStyle();
			likeLabel.SetSmallTitleStyle();
			likeButton.SetImage(UIImage.FromBundle("ic_like.png"), UIControlState.Normal);
			likeButton.SetImage(UIImage.FromBundle("ic_like_active.png"), UIControlState.Selected);

            // TODO: Unhide this button when video saving will be available.
            bookmarkButton.Hidden = true;
		}

		protected override void SetBindings()
		{
			var set = this.CreateBindingSet<PublicationItemCell, PublicationItemViewModel>();

			set.Bind(profileImage.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(profileImage)
				.For(v => v.ImagePath)
				.To(vm => vm.ProfilePhotoUrl);

			set.Bind(profileImage)
				.For(v => v.PlaceholderText)
				.To(vm => vm.ProfileShortName);

			set.Bind(profileNameLabel)
			   .To(vm => vm.ProfileName);

			set.Bind(profileNameLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(videoNameLabel)
				.To(vm => vm.VideoName);

			set.Bind(videoNameLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(publicationInfoLabel)
				.To(vm => vm.VideoInformationText);

			set.Bind(publicationInfoLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(likeButton)
				.To(vm => vm.LikeCommand);

            set.Bind(likeButton)
                .For(UIButtonSelectedTargetBinding.TargetBinding)
                .To(vm => vm.IsLiked);

            set.Bind(likeLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.LikeCommand);

			set.Bind(bookmarkButton.Tap())
			   .For(v => v.Command)
			   .To(vm => vm.BookmarkCommand);

			set.Bind(likeLabel)
				.To(vm => vm.NumberOfLikesText)
				.Mode(MvxBindingMode.OneWay);

			set.Bind(moreButton)
				.To(vm => vm.OpenSettingsCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(shareButton)
	            .To(vm => vm.ShareCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(videoView)
				.For(v => v.BindTap())
				.To(vm => vm.ShowFullScreenVideoCommand);

			set.Bind(stubImageView)
				.For(v => v.ImagePath)
				.To(vm => vm.VideoPlaceholderImageUrl);

			set.Apply();
		}
	}
}
