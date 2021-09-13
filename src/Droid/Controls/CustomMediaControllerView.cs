using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PrankChat.Mobile.Droid.Listeners;

namespace PrankChat.Mobile.Droid.Controls
{
    public class CustomMediaControllerView : FrameLayout, MediaController.IMediaPlayerControl, ExtendedVideoView.IOnVideoViewStateChangedListener, SeekBar.IOnSeekBarChangeListener
    {
        private const int DefaultSecondsViewDelayOnScreen = 4;
        private const long ProgressMultiplier = 1000L;
        private const int UpdateTimeLineMillisecondsDelay = 200;

        private ViewGroup _anchorView;
        private View _controllerView;
        private bool _isViewAdded;

        private DateTime _nextHideTimeStamp;

        private bool _isDragging;
        private TextView _timeTextView;
        private SeekBar _seekBar;
        private ImageView _resumeImageView;
        private ImageView _muteImageView;

        public CustomMediaControllerView(Context context) : base(context)
        {
        }

        public CustomMediaControllerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomMediaControllerView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public CustomMediaControllerView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CustomMediaControllerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private ExtendedVideoView _videoView;
        public ExtendedVideoView VideoView
        {
            get => _videoView;
            set
            {
                _videoView = value;
                _videoView?.SetOnVideoViewStateChangedListener(this);
            }
        }

        public Action<ViewStates> ViewStateChanged { get; set; }

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            set
            {
                _mediaPlayer = value;
                UpdateSoundState(_isMuted);
            }
        }

        public int AudioSessionId => VideoView?.AudioSessionId ?? 0;

        public int BufferPercentage => VideoView?.BufferPercentage ?? 0;

        public int CurrentPosition => VideoView?.CurrentPosition ?? 0;

        public int Duration => VideoView?.Duration ?? 0;

        public bool IsPlaying => VideoView?.IsPlaying ?? false;

        public event EventHandler IsMutedChanged;

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set => UpdateSoundState(value);
        }

        public bool CanPause()
        {
            return VideoView?.CanPause() ?? false;
        }

        public bool CanSeekBackward()
        {
            return VideoView?.CanSeekBackward() ?? false;
        }

        public void SetAnchorView(ViewGroup anchorView)
        {
            if (anchorView is null)
            {
                return;
            }

            _anchorView = anchorView;
            anchorView.SetOnClickListener(new ViewOnClickListener(OnHolderViewClicked));

            var frameParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            RemoveAllViews();

            var view = InflateControllerView();
            AddView(view, frameParams);
            Show();
        }

        public void Show()
        {
            _nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);
            if (_isViewAdded)
            {
                Visibility = ViewStates.Visible;
                ViewStateChanged?.Invoke(Visibility);
                _ = UpdateProgressAsync();
                return;
            }

            if (_anchorView is null)
            {
                return;
            }

            var layoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent,
                                                    ViewGroup.LayoutParams.WrapContent,
                                                    GravityFlags.Bottom);

            _anchorView.AddView(this, layoutParameters);
            _isViewAdded = true;

            ViewStateChanged?.Invoke(Visibility);
            _ = UpdateProgressAsync();
        }

        public void Hide()
        {
            Visibility = ViewStates.Gone;
            ViewStateChanged?.Invoke(Visibility);
        }

        public bool CanSeekForward()
        {
            return VideoView?.CanSeekForward() ?? false;
        }

        public void Pause()
        {
            VideoView?.Pause();
        }

        public void SeekTo(int pos)
        {
            VideoView?.SeekTo(pos);
        }

        public void Start()
        {
            VideoView?.Start();
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            _isDragging = true;
            _nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            _isDragging = false;
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (!_isDragging)
            {
                return;
            }

            _nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);

            var duration = VideoView.Duration;
            var newPosition = duration * progress / ProgressMultiplier;
            VideoView.SeekTo((int)newPosition);
            SetTimeLineLabelValue(newPosition, duration);
        }

        private View InflateControllerView()
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            _controllerView = inflater.Inflate(Resource.Layout.video_controller_layout, null);

            InitControllerView(_controllerView);

            return _controllerView;
        }

        private void InitControllerView(View controllerView)
        {
            _timeTextView = controllerView.FindViewById<TextView>(Resource.Id.timeline_text_view);
            _seekBar = controllerView.FindViewById<SeekBar>(Resource.Id.play_progress_seek_bar);
            _resumeImageView = controllerView.FindViewById<ImageView>(Resource.Id.resume_image_view);
            _muteImageView = controllerView.FindViewById<ImageView>(Resource.Id.mute_image_view);

            _resumeImageView.SetOnClickListener(new ViewOnClickListener(OnResumeImageClicked));
            _muteImageView.SetOnClickListener(new ViewOnClickListener(OnMuteImageClicked));

            _seekBar.Max = (int)ProgressMultiplier;
            _seekBar.SetOnSeekBarChangeListener(this);
        }

        private void OnMuteImageClicked(View view)
        {
            _nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);
            IsMuted = !IsMuted;
            IsMutedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSoundState(bool value)
        {
            _isMuted = value;

            if (MediaPlayer is null)
            {
                return;
            }

            if (value)
            {
                Mute();
                return;
            }

            Unmute();
        }

        private void Mute()
        {
            MediaPlayer.SetVolume(0,0);
            _muteImageView.SetImageResource(Resource.Drawable.ic_without_sound);
        }

        private void Unmute()
        {
            MediaPlayer.SetVolume(1, 1);
            _muteImageView.SetImageResource(Resource.Drawable.ic_sound);
        }

        private void OnResumeImageClicked(View view)
        {
            _nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);

            if (VideoView.IsPlaying)
            {
                VideoView.Pause();
                return;
            }

            VideoView.Start();
        }

        private void SetProgress()
        {
            var position = VideoView.CurrentPosition;
            var duration = VideoView.Duration;
            var targetPosition = ProgressMultiplier * position / duration;

            _seekBar.Progress = (int)targetPosition;
            SetTimeLineLabelValue(position, duration);

            var percent = VideoView.BufferPercentage;
            _seekBar.SecondaryProgress = percent * 10;
        }

        private async Task UpdateProgressAsync()
        {
            while (Visibility == ViewStates.Visible)
            {
                if (_nextHideTimeStamp <= DateTime.Now)
                {
                    Hide();
                }

                await Task.Delay(UpdateTimeLineMillisecondsDelay);
                if (!_isDragging)
                {
                    SetProgress();
                }

                if (IsPlaying || _videoView.Duration == 0)
                {
                    continue;
                }

                _resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_play_circle_outline);
            }
        }

        private void SetTimeLineLabelValue(long currentPosition, long totalMiliseconds)
        {
            _timeTextView.Text = $"{GetTimeLineText(currentPosition)}/{GetTimeLineText(totalMiliseconds)}";
        }

        private string GetTimeLineText(long timeMiliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(timeMiliseconds);

            if (timeSpan.Hours > 0)
            {
                return timeSpan.ToString("HH\\:mm\\:ss");
            }

            return timeSpan.ToString("mm\\:ss");
        }

        private void OnHolderViewClicked(View view)
        {
            if (Visibility == ViewStates.Gone)
            {
                Show();
                return;
            }

            Hide();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_videoView != null)
                {
                    _videoView.SetOnVideoViewStateChangedListener(null);
                    _videoView.Dispose();
                }

                if (_anchorView != null)
                {
                    _anchorView.SetOnClickListener(null);
                    _anchorView.Dispose();
                }

                if (MediaPlayer != null)
                {
                    MediaPlayer.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        void ExtendedVideoView.IOnVideoViewStateChangedListener.Pause()
        {
            if (_resumeImageView is null)
            {
                return;
            }

            _resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_play_circle_outline);
        }

        void ExtendedVideoView.IOnVideoViewStateChangedListener.Start()
        {
            if (_resumeImageView is null)
            {
                return;
            }

            _resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_pause_circle_outline);
        }

        void ExtendedVideoView.IOnVideoViewStateChangedListener.StopPlayback()
        {
            if (_resumeImageView is null)
            {
                return;
            }

            _resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_play_circle_outline);
        }
    }
}
