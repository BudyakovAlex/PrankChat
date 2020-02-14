using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.Controls
{
    public class CustomMediaControllerView : FrameLayout, MediaController.IMediaPlayerControl, SeekBar.IOnSeekBarChangeListener
    {
        private const int DefaultSecondsViewDelayOnScreen = 4;

        private ViewGroup anchorView;
        private View controllerView;
        private bool isViewAdded;

        private DateTime nextHideTimeStamp;
        private bool isDragging;
        private TextView timeTextView;
        private SeekBar seekBar;
        private ImageView resumeImageView;
        private ImageView muteImageView;

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

        public MediaController.IMediaPlayerControl MediaPlayer { get; set; }

        public int AudioSessionId => MediaPlayer?.AudioSessionId ?? 0;

        public int BufferPercentage => MediaPlayer?.BufferPercentage ?? 0;

        public int CurrentPosition => MediaPlayer?.CurrentPosition ?? 0;

        public int Duration => MediaPlayer?.Duration ?? 0;

        public bool IsPlaying => MediaPlayer?.IsPlaying ?? false;

        public bool CanPause()
        {
            return MediaPlayer?.CanPause() ?? false;
        }

        public bool CanSeekBackward()
        {
            return MediaPlayer?.CanSeekBackward() ?? false;
        }

        public void SetAnchorView(ViewGroup anchorView)
        {
            this.anchorView = anchorView;

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

            _ = UpdateProgressAsync();
        }

        public void Hide()
        {
            Visibility = ViewStates.Gone;
        }

        public bool CanSeekForward()
        {
            return MediaPlayer?.CanSeekForward() ?? false;
        }

        public void Pause()
        {
            MediaPlayer?.Pause();
        }

        public void SeekTo(int pos)
        {
            MediaPlayer?.SeekTo(pos);
        }

        public void Start()
        {
            MediaPlayer?.Start();
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            isDragging = true;
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

            var duration = MediaPlayer.Duration;
            var newPosition = duration * progress / 1000L;
            MediaPlayer.SeekTo((int)newPosition);
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

            seekBar.Max = 1000;
            seekBar.SetOnSeekBarChangeListener(this);
        }

        private void OnMuteImageClicked(View view)
        {
            
        }

        private void OnResumeImageClicked(View view)
        {
            if (MediaPlayer.IsPlaying)
            {
                MediaPlayer.Pause();
                return;
            }

            MediaPlayer.Start();
        }

        private void SetPorgress()
        {
            var position = MediaPlayer.CurrentPosition;
            var duration = MediaPlayer.Duration;
            if (duration > 0)
            {
                var targetPosition = 1000L * position / duration;
                seekBar.Progress = (int)targetPosition;
                SetTimeLineLabelValue(position, duration);
            }

            var percent = MediaPlayer.BufferPercentage;
            seekBar.SecondaryProgress = percent * 10;
        }

        private async Task UpdateProgressAsync()
        {
            while (Visibility == ViewStates.Visible)
            {
                await Task.Delay(200);
                SetPorgress();
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
    }
}