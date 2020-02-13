﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Video
{
    [Activity]
    [MvxActivityPresentation]
    public class FullScreenVideoView : BaseView<FullScreenVideoViewModel>
    {
        private VideoView videoView;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            var decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.HideNavigation
                | SystemUiFlags.Fullscreen
                | SystemUiFlags.LayoutFullscreen
                | SystemUiFlags.LayoutHideNavigation
                | SystemUiFlags.Immersive);

            base.OnCreate(bundle, Resource.Layout.activity_full_screen_video);
            RequestedOrientation = ScreenOrientation.FullSensor;
        }

        protected override void SetViewProperties()
        {
            videoView = FindViewById<VideoView>(Resource.Id.video_view);
            var mediaController = new MediaController(this);
            mediaController.SetMediaPlayer(videoView);
            mediaController.SetAnchorView(videoView);

            videoView.SetMediaController(mediaController);
            videoView.RequestFocus();
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<FullScreenVideoView, FullScreenVideoViewModel>();

            bindingSet.Bind(videoView).For(VideoUrlTargetBinding.PropertyName).To(vm => vm.VideoUrl);

            bindingSet.Apply();
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
