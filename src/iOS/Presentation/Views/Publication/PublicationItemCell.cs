using System;
using AVFoundation;
using AVKit;
using CoreGraphics;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Presentation.Localization;
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
		private AVPlayerViewController _avPlayerViewController;

		static PublicationItemCell()
		{
			EstimatedHeight = 334;
		}

		protected PublicationItemCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public void PlayVideo(string uri)
		{
			var service = ViewModel.VideoPlayerService;
			if (service.Player.IsPlaying)
				return;

			service.Stop();
			service.Player.SetPlatformVideoPlayerContainer(_avPlayerViewController);
			service.Play(uri);
		}

		public void ContinueVideo()
		{
			var service = ViewModel.VideoPlayerService;
			if (!service.Player.IsPlaying)
				return;

			service.Play();
		}

		public void PauseVideo()
		{
			var service = ViewModel.VideoPlayerService;
			if (!service.Player.IsPlaying)
				return;

			service.Pause();
		}

		public void StopVideo()
		{
			if (!ViewModel.VideoPlayerService.Player.IsPlaying)
				return;

			ViewModel.VideoPlayerService.Stop();
			_avPlayerViewController.Player = null;
		}

		public CGRect GetVideoBounds(UITableView tableView)
		{
			return videoView.ConvertRectToView(videoView.Bounds, tableView);
		}

		public override void PrepareForReuse()
		{
			StopVideo();
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
			shareLabel.SetSmallTitleStyle();
			shareLabel.Text = Resources.Share;
			likeButton.SetImage(UIImage.FromBundle("ic_like.png"), UIControlState.Normal);
			likeButton.SetImage(UIImage.FromBundle("ic_like_active.png"), UIControlState.Selected);

            InitializeVideoControl();
		}

		protected override void SetBindings()
		{
			var set = this.CreateBindingSet<PublicationItemCell, PublicationItemViewModel>();

			set.Bind(profileImage)
				.For(v => v.DownsampleWidth)
				.To(vm => vm.DownsampleWidth)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileImage)
				.For(v => v.Transformations)
				.To(vm => vm.Transformations)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileImage.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

			set.Bind(profileImage)
				.For(v => v.ImagePath)
				.To(vm => vm.ProfilePhotoUrl)
				.WithConversion<PlaceholderImageConverter>()
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
				.To(vm => vm.VideoInformationText)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(publicationInfoLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShowDetailsCommand);

            // TODO: we should probably move this functionality to the mute button
			//set.Bind(_avPlayerViewController.View.Tap())
			//	.For(v => v.Command)
			//	.To(vm => vm.ToggleSoundCommand);

			set.Bind(soundImageView)
				.For(v => v.Hidden)
				.To(vm => vm.HasSoundTurnOn);

			set.Bind(likeButton)
				.To(vm => vm.LikeCommand);

            set.Bind(likeButton)
                .For(UIButtonSelectedTargetBinding.TargetBinding)
                .To(vm => vm.IsLiked);

            set.Bind(likeLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.LikeCommand);

			set.Bind(likeLabel)
				.To(vm => vm.NumberOfLikesText)
				.Mode(MvxBindingMode.OneWay);

			set.Bind(moreButton)
				.To(vm => vm.OpenSettingsCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(shareButton)
				.To(vm => vm.ShareCommand)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(shareLabel.Tap())
				.For(v => v.Command)
				.To(vm => vm.ShareCommand);

			set.Bind(profileShortNameLabel)
				.To(vm => vm.ProfileShortName)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(profileShortNameLabel)
				.For(v => v.BindHidden())
				.To(vm => vm.ProfilePhotoUrl)
				.Mode(MvxBindingMode.OneTime);

			set.Bind(videoView)
				.For(v => v.BindTap())
				.To(vm => vm.ShowFullScreenVideoCommand);

			set.Apply();
		}

		private void InitializeVideoControl()
		{
			_avPlayerViewController = new AVPlayerViewController();
			_avPlayerViewController.View.Frame = new CGRect(0, 0, videoView.Frame.Width, videoView.Frame.Height);
			_avPlayerViewController.ShowsPlaybackControls = false;
			_avPlayerViewController.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
			videoView.Add(_avPlayerViewController.View);
		}
	}
}