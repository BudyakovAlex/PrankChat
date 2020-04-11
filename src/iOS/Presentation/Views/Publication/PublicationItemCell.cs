using System;
using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Converters;
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

		protected override UIView RootProcessingBackgroundView => placeProcessingOverlay;

		protected override UIView ProcessingBackgroundView => processingBackgroundView;

		protected override UIActivityIndicatorView ProcessingActivityIndicator => processingIndicatorView;

		protected override UILabel ProcessingLabel => processingLabel;

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

			var gradient = new CAGradientLayer
			{
				CornerRadius = 10,
				MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner,
				StartPoint = new CGPoint(0f, 1f),
				EndPoint = new CGPoint(1f, 1f),
				Colors = new[] { Theme.Color.CompetitionPhaseVotingSecondary.CGColor, Theme.Color.CompetitionPhaseVotingPrimary.CGColor }
			};

			placeProcessingOverlay.Layer.InsertSublayer(gradient, 0);
			placeProcessingOverlay.Layer.CornerRadius = 10;
			processingBackgroundView.Layer.CornerRadius = 8;
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

			set.Bind(placeProcessingOverlay)
			   .For(v => v.BindVisible())
			   .To(vm => vm.IsVideoProcessing);

			set.Bind(videoView)
			   .For(v => v.BindHidden())
			   .To(vm => vm.IsVideoProcessing);

			set.Bind(this)
			   .For(v => v.CanShowStub)
			   .To(vm => vm.IsVideoProcessing)
               .WithConversion<MvxInvertedBooleanConverter>();

			set.Apply();
		}
	}
}
