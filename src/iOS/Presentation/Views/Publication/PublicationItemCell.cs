using System;
using AVFoundation;
using AVKit;
using CoreGraphics;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Publication
{
    public partial class PublicationItemCell : BaseTableCell<PublicationItemCell, PublicationItemViewModel>
	{
		public AVPlayerViewController AVPlayerViewControllerInstance { get; private set; }

		static PublicationItemCell()
		{
			EstimatedHeight = 334;
		}

		protected PublicationItemCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public CGRect GetVideoBounds(UITableView tableView)
		{
			return videoView.ConvertRectToView(videoView.Bounds, tableView);
		}

		public override void PrepareForReuse()
		{
			PauseVideo();
			base.PrepareForReuse();
		}

		protected override void Dispose(bool disposing)
		{
			StopVideo();
			base.Dispose(disposing);
		}

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

            InitializeVideoControl();
		}

		protected override void SetBindings()
		{
			var set = this.CreateBindingSet<PublicationItemCell, PublicationItemViewModel>();

			set.Bind(profileImage.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(profileImage)
				.For(v => v.ImagePath)
				.To(vm => vm.ProfilePhotoUrl)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileImage)
	            .For(v => v.PlaceholderText)
	            .To(vm => vm.ProfileShortName)
	            .Mode(MvxBindingMode.OneTime);

			set.Bind(profileNameLabel)
				.To(vm => vm.ProfileName)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileNameLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(videoNameLabel)
				.To(vm => vm.VideoName)
				.Mode(MvxBindingMode.OneTime);

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

			set.Apply();
		}

		private void StopVideo()
		{
			if (AVPlayerViewControllerInstance == null)
				return;

			AVPlayerViewControllerInstance.Player?.Pause();
			AVPlayerViewControllerInstance.Player = null;
		}

        private void PauseVideo()
        {
			if (AVPlayerViewControllerInstance == null)
				return;

			AVPlayerViewControllerInstance.Player?.Pause();
		}

		private void InitializeVideoControl()
		{
			AVPlayerViewControllerInstance = new AVPlayerViewController();
			AVPlayerViewControllerInstance.View.Frame = new CGRect(0, 0, videoView.Frame.Width, videoView.Frame.Height);
			AVPlayerViewControllerInstance.ShowsPlaybackControls = false;
			AVPlayerViewControllerInstance.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
			videoView.Add(AVPlayerViewControllerInstance.View);
		}
	}
}
