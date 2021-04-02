using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Video
{
    public class FullScreenVideoViewModel : BasePageViewModel<FullScreenVideoParameter, bool>
    {
        private readonly IUsersManager _usersManager;

        private bool _isReloadNeeded;
        private int _index;

        private BaseVideoItemViewModel[] _videos;
        private IDisposable _likeChangedSubscription;
        private IDisposable _dislikeChangedSubscription;

        public FullScreenVideoViewModel(IUsersManager usersManager)
        {
            Interaction = new MvxInteraction();

            _usersManager = usersManager;

            ShareCommand = this.CreateCommand(ShareAsync);
            MoveNextCommand = this.CreateCommand(MoveNext);
            MovePreviousCommand = this.CreateCommand(MovePrevious);

            OpenCommentsCommand = this.CreateRestrictedCommand(
                ShowCommentsAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigateByRestrictionsAsync);

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => UserSessionProvider.User != null,
                handleFunc: NavigateByRestrictionsAsync);
        }

        protected override bool DefaultResult => _isReloadNeeded;

        public IMvxAsyncCommand ShareCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public IMvxCommand MoveNextCommand { get; }

        public IMvxCommand MovePreviousCommand { get; }

        public BaseVideoItemViewModel CurrentVideo { get; private set; }

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
            get => Preferences.Get(Constants.Keys.MuteStateKey, false);
            set
            {
                Preferences.Set(Constants.Keys.MuteStateKey, value);
                RaisePropertyChanged();
            }
        }

        public string NumberOfLikesPresentation => CurrentVideo.NumberOfLikes.ToCountString();

        public string NumberOfDislikesPresentation => CurrentVideo.NumberOfLikes.ToCountString();

        public string NumberOfCommentsPresentation => CurrentVideo.NumberOfComments.ToCountString();

        public override void Prepare(FullScreenVideoParameter parameter)
        {
            _videos = parameter.Videos;
            _index = parameter.StartIndex;

            RefreshCurrentVideoState();
        }

        private void OnLikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfLikesPresentation));
            _isReloadNeeded = true;
        }

        private void OnDislikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfDislikesPresentation));
            _isReloadNeeded = true;
        }

        private Task NavigateByRestrictionsAsync()
        {
            Interaction.Raise();
            return NavigationManager.NavigateAsync<LoginViewModel>();
        }

        private async Task OpenUserProfileAsync()
        {
            if (CurrentVideo.UserId == 0 ||
                CurrentVideo.UserId == UserSessionProvider.User.Id)
            {
                return;
            }

            Interaction.Raise();

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return;
            }

            var shouldRefresh = await NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(CurrentVideo.UserId);
            if (!shouldRefresh)
            {
                return;
            }

            var user = await _usersManager.GetUserAsync(CurrentVideo.UserId);
            CurrentVideo.IsSubscribedToUser = user.IsSubscribed;
        }

        private async Task ShowCommentsAsync()
        {
            Interaction.Raise();

            var commentsCount = await NavigationManager.NavigateAsync<CommentsViewModel, int, int>(CurrentVideo.VideoId);
            CurrentVideo.NumberOfComments = commentsCount > 0 ? commentsCount : CurrentVideo.NumberOfComments;
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
            if (_index == _videos.Length - 1)
            {
                return;
            }

            _index++;
            RefreshCurrentVideoState();
        }

        private void RefreshCurrentVideoState()
        {
            if (CurrentVideo != null)
            {
                CurrentVideo.FullVideoPlayer.Stop();
                _likeChangedSubscription?.Dispose();
                _likeChangedSubscription = null;

                _dislikeChangedSubscription?.Dispose();
                _dislikeChangedSubscription = null;
            }

            CurrentVideo = _videos.ElementAtOrDefault(_index);
            if (CurrentVideo is null)
            {
                throw new ArgumentOutOfRangeException($"Failed to load video on index {_index}");
            }

            _likeChangedSubscription = CurrentVideo.SubscribeToPropertyChanged(nameof(BaseVideoItemViewModel.IsLiked), OnLikeChanged);
            _dislikeChangedSubscription = CurrentVideo.SubscribeToPropertyChanged(nameof(BaseVideoItemViewModel.IsDisliked), OnDislikeChanged);

            CurrentVideo.RaiseAllPropertiesChanged();
            RaiseAllPropertiesChanged();

            CurrentVideo.FullVideoPlayer.Play();
        }

        private Task ShareAsync()
        {
            Interaction.Raise();

            return Share.RequestAsync(new ShareTextRequest
            {
                Uri = CurrentVideo.ShareLink,
                Title = Resources.ShareDialog_LinkShareTitle
            });
        }
    }
}