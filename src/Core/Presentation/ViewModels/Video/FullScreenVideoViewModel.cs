using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Video
{
    public class FullScreenVideoViewModel : LikeableViewModel, IMvxViewModel<FullScreenVideoParameter, bool>
    {
        private readonly IPlatformService _platformService;

        private bool _isReloadNeeded;
        private string _shareLink;
        private int _index;

        private FullScreenVideoDataModel _currentVideo;

        private List<FullScreenVideoDataModel> _videos;

        public FullScreenVideoViewModel(IPlatformService platformService)
        {
            Interaction = new MvxInteraction();

            _platformService = platformService;
            ShareCommand = new MvxAsyncCommand(ShareAsync);
            MoveNextCommand = new MvxCommand(MoveNext);
            MovePreviousCommand = new MvxCommand(MovePrevious);
            OpenCommentsCommand = new MvxRestrictedAsyncCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigateByRestrictionsAsync);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => SettingsService.User != null, handleFunc: NavigateByRestrictionsAsync);
        }

        public IMvxAsyncCommand ShareCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public IMvxCommand MoveNextCommand { get; }

        public IMvxCommand MovePreviousCommand { get; }

        public MvxInteraction Interaction { get; }

        public MvxRestrictedAsyncCommand OpenCommentsCommand { get; }

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            private set => SetProperty(ref _videoUrl, value);
        }

        public bool IsMuted
        {
            get => Xamarin.Essentials.Preferences.Get(Constants.Keys.MuteStateKey, false);
            set
            {
                Xamarin.Essentials.Preferences.Set(Constants.Keys.MuteStateKey, value);
                RaisePropertyChanged();
            }
        }

        public string VideoName { get; private set; }

        public string Description { get; private set; }

        public string ProfilePhotoUrl { get; private set; }

        public string ProfileShortName { get; private set; }

        public bool IsLikeFlowAvailable { get; private set; }

        public bool IsSubscribed { get; private set; }

        public long? NumberOfComments { get; private set; }

        public string NumberOfLikesPresentation => NumberOfLikes.ToCountString();

        public string NumberOfDislikesPresentation => NumberOfDislikes.ToCountString();

        public string NumberOfCommentsPresentation => NumberOfComments.ToCountString();

        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public void Prepare(FullScreenVideoParameter parameter)
        {
            _videos = parameter.Videos;
            _index = parameter.Index;

            RefreshCurrentVideoState();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isReloadNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        protected override void OnLike()
        {
            if (!IsLikeFlowAvailable)
            {
                return;
            }

            base.OnLike();
        }

        protected override void OnDislike()
        {
            if (!IsLikeFlowAvailable)
            {
                return;
            }

            base.OnDislike();
        }

        protected override void OnLikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfLikesPresentation));
            _isReloadNeeded = true;
        }

        protected override void OnDislikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfDislikesPresentation));
            _isReloadNeeded = true;
        }

        private Task NavigateByRestrictionsAsync()
        {
            Interaction.Raise();
            return NavigationService.ShowLoginView();
        }

        private async Task OpenUserProfileAsync()
        {
            if (_currentVideo?.UserId is null ||
                _currentVideo.UserId == 0 ||
                _currentVideo.UserId == SettingsService.User.Id)
            {
                return;
            }

            Interaction.Raise();

            var shouldRefresh = await NavigationService.ShowUserProfile(_currentVideo.UserId);
            if (!shouldRefresh)
            {
                return;
            }

            var user = await ApiService.GetUserAsync(_currentVideo.UserId);
            _currentVideo.IsSubscribed = user.IsSubscribed;
            IsSubscribed = _currentVideo.IsSubscribed;

            await RaisePropertyChanged(nameof(IsSubscribed));
        }

        private async Task ShowCommentsAsync()
        {
            Interaction.Raise();

            var commentsCount = await NavigationService.ShowCommentsView(VideoId);
            NumberOfComments = commentsCount > 0 ? commentsCount : NumberOfComments;
            await RaisePropertyChanged(nameof(NumberOfCommentsPresentation));
            _isReloadNeeded = true;
        }

        private void MovePrevious()
        {
            if (_index == 0)
            {
                return;
            }

            _index--;
            RefreshCurrentVideoState();
        }

        private void MoveNext()
        {
            if (_index - 1 == _videos.Count)
            {
                return;
            }

            _index++;
            RefreshCurrentVideoState();
        }

        private void RefreshCurrentVideoState()
        {
            _currentVideo = _videos.ElementAtOrDefault(_index);
            if (_currentVideo is null)
            {
                return;
            }

            VideoId = _currentVideo.VideoId;
       
            VideoName = _currentVideo.VideoName;
            Description = _currentVideo.Description;
            ProfilePhotoUrl = _currentVideo.ProfilePhotoUrl;
            ProfileShortName = _currentVideo.UserShortName;
            NumberOfLikes = _currentVideo.NumberOfLikes;
            NumberOfDislikes = _currentVideo.NumberOfDislikes;
            IsLikeFlowAvailable = _currentVideo.IsLikeFlowAvailable;
            NumberOfComments = _currentVideo.NumberOfComments;
            IsSubscribed = _currentVideo.IsSubscribed;

            _shareLink = _currentVideo.ShareLink;
            RaiseAllPropertiesChanged();

            IsLiked = _currentVideo.IsLiked;
            IsDisliked = _currentVideo.IsDisliked;
            VideoUrl = _currentVideo.VideoUrl;
        }

        private Task ShareAsync()
        {
            Interaction.Raise();
            return _platformService.ShareUrlAsync(Resources.ShareDialog_LinkShareTitle, _shareLink);
            //TODO: remove comments when logic will be ready
            //return DialogService.ShowShareDialogAsync(_shareLink);
        }
    }
}