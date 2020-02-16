using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.Controls
{
    public class CustomMediaControllerView : FrameLayout, MediaController.IMediaPlayerControl, ExtendedVideoView.IOnVideoViewStateChangedListener, SeekBar.IOnSeekBarChangeListener
    {
        private const int DefaultSecondsViewDelayOnScreen = 4;
        private const long ProgressMultiplier = 1000L;
        private const int UpdateTimeLineMillisecondsDelay = 200;

        private ViewGroup anchorView;
        private View controllerView;
        private bool isViewAdded;

        private DateTime nextHideTimeStamp;

        private bool isDragging;
        private TextView timeTextView;
        private SeekBar seekBar;
        private ImageView resumeImageView;
        private ImageView muteImageView;

        private ExtendedVideoView videoView;
        private bool isMuted;

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

        public ExtendedVideoView VideoView
        {
            get => videoView;
            set
            {
                videoView = value;
                if (videoView is null)
                {
                    return;
                }

                videoView.SetOnVideoViewStateChangedListener(this);
            }
        }

        public Action<ViewStates> ViewStateChanged { get; set; }

        public MediaPlayer MediaPlayer { get; set; }

        public int AudioSessionId => VideoView?.AudioSessionId ?? 0;

        public int BufferPercentage => VideoView?.BufferPercentage ?? 0;

        public int CurrentPosition => VideoView?.CurrentPosition ?? 0;

        public int Duration => VideoView?.Duration ?? 0;

        public bool IsPlaying => VideoView?.IsPlaying ?? false;

        public bool IsMuted
        {
            get => isMuted;
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

            this.anchorView = anchorView;
            anchorView.SetOnClickListener(new ViewOnClickListener(OnHolderViewClicked));

            var frameParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            RemoveAllViews();

            var view = InflateControllerView();
            AddView(view, frameParams);
            Show();
        }

        public void Show()
        {
            nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);
            if (isViewAdded)
            {
                Visibility = ViewStates.Visible;
                ViewStateChanged?.Invoke(Visibility);
                _ = UpdateProgressAsync();
                return;
            }

            if (anchorView is null)
            {
                return;
            }

            var layoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent,
                                                    ViewGroup.LayoutParams.WrapContent,
                                                    GravityFlags.Bottom);

            anchorView.AddView(this, layoutParameters);
            isViewAdded = true;

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
            isDragging = true;
            nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            isDragging = false;
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (!isDragging)
            {
                return;
            }

            nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);

            var duration = VideoView.Duration;
            var newPosition = duration * progress / ProgressMultiplier;
            VideoView.SeekTo((int)newPosition);
            SetTimeLineLabelValue(newPosition, duration);
        }

        private View InflateControllerView()
        {
            var inflater = Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            controllerView = inflater.Inflate(Resource.Layout.video_controller_layout, null);

            InitControllerView(controllerView);

            return controllerView;
        }

        private void InitControllerView(View controllerView)
        {
            timeTextView = controllerView.FindViewById<TextView>(Resource.Id.timeline_text_view);
            seekBar = controllerView.FindViewById<SeekBar>(Resource.Id.play_progress_seek_bar);
            resumeImageView = controllerView.FindViewById<ImageView>(Resource.Id.resume_image_view);
            muteImageView = controllerView.FindViewById<ImageView>(Resource.Id.mute_image_view);

            resumeImageView.SetOnClickListener(new ViewOnClickListener(OnResumeImageClicked));
            muteImageView.SetOnClickListener(new ViewOnClickListener(OnMuteImageClicked));

            seekBar.Max = (int)ProgressMultiplier;
            seekBar.SetOnSeekBarChangeListener(this);
        }

        private void OnMuteImageClicked(View view)
        {
            nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);
            IsMuted = !IsMuted;
        }

        private void UpdateSoundState(bool value)
        {
            if (value == isMuted)
            {
                return;
            }

            isMuted = value;

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
            muteImageView.SetImageResource(Resource.Drawable.ic_sound);
        }

        private void Unmute()
        {
            MediaPlayer.SetVolume(1, 1);
            muteImageView.SetImageResource(Resource.Drawable.ic_without_sound);
        }

        private void OnResumeImageClicked(View view)
        {
            nextHideTimeStamp = DateTime.Now.AddSeconds(DefaultSecondsViewDelayOnScreen);

            if (VideoView.IsPlaying)
            {
                VideoView.Pause();
                return;
            }

            VideoView.Start();
        }

        private void SetPorgress()
        {
            var position = VideoView.CurrentPosition;
            var duration = VideoView.Duration;
            var targetPosition = ProgressMultiplier * position / duration;

            seekBar.Progress = (int)targetPosition;
            SetTimeLineLabelValue(position, duration);

            var percent = VideoView.BufferPercentage;
            seekBar.SecondaryProgress = percent * 10;
        }

        private async Task UpdateProgressAsync()
        {
            while (Visibility == ViewStates.Visible)
            {
                if (nextHideTimeStamp <= DateTime.Now)
                {
                    Hide();
                }

                await Task.Delay(UpdateTimeLineMillisecondsDelay);
                if (!isDragging)
                {
                    SetPorgress();
                }

                if (IsPlaying || videoView.Duration == 0)
                {
                    continue;
                }

                resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_play_circle_outline);
            }
        }

        private void SetTimeLineLabelValue(long currentPosition, long totalMiliseconds)
        {
            timeTextView.Text = $"{GetTimeLineText(currentPosition)}/{GetTimeLineText(totalMiliseconds)}";
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
                if (videoView != null)
                {
                    videoView.SetOnVideoViewStateChangedListener(null);
                    videoView.Dispose();
                }

                if (anchorView != null)
                {
                    anchorView.SetOnClickListener(null);
                    anchorView.Dispose();
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
            if (resumeImageView is null)
            {
                return;
            }

            resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_play_circle_outline);
        }

        void ExtendedVideoView.IOnVideoViewStateChangedListener.Start()
        {
            if (resumeImageView is null)
            {
                return;
            }

            resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_pause_circle_outline);
        }

        void ExtendedVideoView.IOnVideoViewStateChangedListener.StopPlayback()
        {
            if (resumeImageView is null)
            {
                return;
            }

            resumeImageView.SetImageResource(Resource.Drawable.ic_mdi_play_circle_outline);
        }
    }
}