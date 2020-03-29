using System;
using AVFoundation;
using AVKit;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Base
{
    public abstract class BaseVideoTableCell : BaseTableCell
	{
		private const string PlayerStatusObserverKey = "status";

		private NSObject _playerPerdiodicTimeObserver;
		private bool _isObserverRemoved;

		protected BaseVideoTableCell(IntPtr handle)
			: base(handle)
		{
		}

		public IVideoItemViewModel ViewModel => BindingContext.DataContext as IVideoItemViewModel;

		public AVPlayerViewController AVPlayerViewControllerInstance { get; private set; }

		protected abstract UIView VideoView { get; }

        protected abstract UIActivityIndicatorView LoadingActivityIndicator { get; }

		protected abstract UIImageView StubImageView { get; }

		public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
		{
			if (keyPath != PlayerStatusObserverKey)
			{
				return;
			}

			if (AVPlayerViewControllerInstance.Player != null &&
				AVPlayerViewControllerInstance.Player.Status == AVPlayerStatus.ReadyToPlay)
			{
                LoadingActivityIndicator.Hidden = true;
                StubImageView.Hidden = true;
            }
		}

		public override void PrepareForReuse()
		{
			ShowStub();

			if (!_isObserverRemoved)
			{
				AVPlayerViewControllerInstance.Player?.RemoveTimeObserver(_playerPerdiodicTimeObserver);
				_playerPerdiodicTimeObserver = null;
			}

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

			VideoView.SetPreviewStyle();
			InitializeVideoControl();
		}

		public CGRect GetVideoBounds(UITableView tableView) =>
            VideoView.ConvertRectToView(VideoView.Bounds, tableView);

		public void AddObserverForPeriodicTime()
		{
            LoadingActivityIndicator.Hidden = false;
            LoadingActivityIndicator.StartAnimating();

            _playerPerdiodicTimeObserver = AVPlayerViewControllerInstance.Player?.AddPeriodicTimeObserver(
				new CMTime(1, 2),
				DispatchQueue.MainQueue,
				PlayerTimeChanged);
		}

		public void ShowStub()
		{
            StubImageView.Hidden = false;
            LoadingActivityIndicator.Hidden = true;
        }

		private void PlayerTimeChanged(CMTime obj)
		{
			if (obj.Value > 0)
			{
				_isObserverRemoved = true;
				if (_playerPerdiodicTimeObserver != null)
				{
                    LoadingActivityIndicator.Hidden = true;
                    StubImageView.Hidden = true;

                    AVPlayerViewControllerInstance.Player?.RemoveTimeObserver(_playerPerdiodicTimeObserver);
					_playerPerdiodicTimeObserver = null;
				}
			}
		}

		private void StopVideo()
		{
			ViewModel?.VideoPlayerService?.Stop();

			if (AVPlayerViewControllerInstance != null)
			{
				AVPlayerViewControllerInstance.Player = null;
			}
		}

		private void InitializeVideoControl()
		{
			AVPlayerViewControllerInstance = new AVPlayerViewController();
			AVPlayerViewControllerInstance.View.Frame = new CGRect(0, 0, VideoView.Frame.Width, VideoView.Frame.Height);
			AVPlayerViewControllerInstance.ShowsPlaybackControls = false;
			AVPlayerViewControllerInstance.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
			VideoView.Add(AVPlayerViewControllerInstance.View);
		}
	}

    public abstract class BaseVideoTableCell<TCell, TViewModel> : BaseVideoTableCell
        where TCell : BaseVideoTableCell
		where TViewModel : class, IVideoItemViewModel
    {
        protected BaseVideoTableCell(IntPtr handle)
            : base(handle)
        {
        }

		static BaseVideoTableCell()
		{
			CellId = typeof(TCell).Name;
			Nib = UINib.FromName(CellId, NSBundle.MainBundle);
		}

		public static string CellId { get; }

		public static UINib Nib { get; }

		public new TViewModel ViewModel => BindingContext.DataContext as TViewModel;
    }
}
