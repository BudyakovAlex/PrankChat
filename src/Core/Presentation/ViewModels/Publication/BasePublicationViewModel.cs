using System;
using System.Threading.Tasks;
using MediaManager;
using MediaManager.Video;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class BasePublicationViewModel : BaseViewModel
    {
        private long _numberOfViews;
        private DateTime _publicationDate;
        private long _numberOfLikes;

        #region Profile

        public string ProfileName { get; set; } = "Name one";

        public string ProfilePhotoUrl { get; set; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        #endregion

        #region Video

        public string VideoInformationText => $"{_numberOfViews} просмотров {_publicationDate.ToShortDateString()} месяцев назад";

        public string VideoName { get; set; } = "Name video one";

        public string VideoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        private bool _hasSoundTurnOn;
        public bool HasSoundTurnOn
        {
            get => _hasSoundTurnOn;
            set => SetProperty(ref _hasSoundTurnOn, value);
        }

        #endregion

        public string NumberOfLikesText => $"{Resources.Like} {_numberOfLikes}";

        #region Commands

        public MvxAsyncCommand LikeCommand => new MvxAsyncCommand(OnLikeAsync);

        public MvxAsyncCommand BookmarkCommand => new MvxAsyncCommand(OnBookmarkAsync);

        public MvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OnOpenSettingAsync);

        public MvxCommand ToggleSoundCommand => new MvxCommand(OnToggleSound);

        public MvxCommand<IVideoView> PlayVideoCommand => new MvxCommand<IVideoView>(OnPlayVideo);

        #endregion

        public BasePublicationViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public BasePublicationViewModel(INavigationService navigationService,
                                        string profileName,
                                        string profilePhotoUrl,
                                        string videoName,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes)
            : base (navigationService)
        {
            ProfileName = profileName;
            ProfilePhotoUrl = profilePhotoUrl;
            VideoName = videoName;
            VideoUrl = videoUrl;

            _numberOfViews = numberOfViews;
            _publicationDate = publicationDate;
            _numberOfLikes = numberOfLikes;

            CrossMediaManager.Current.Volume.Muted = HasSoundTurnOn;
        }

        private Task OnLikeAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnBookmarkAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnOpenSettingAsync()
        {
            return Task.CompletedTask;
        }

        private void OnToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;
            CrossMediaManager.Current.Volume.Muted = HasSoundTurnOn;
        }

        private void OnPlayVideo(IVideoView videoView)
        {
            if (CrossMediaManager.Current.IsPlaying())
                CrossMediaManager.Current.Stop();

            CrossMediaManager.Current.MediaPlayer.VideoView = videoView;
            CrossMediaManager.Current.Play(VideoUrl);
        }
    }
}
