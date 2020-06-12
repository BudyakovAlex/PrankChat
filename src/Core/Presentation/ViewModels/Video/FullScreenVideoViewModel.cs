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

        private List<FullScreenVideoDataModel> _videos;

        public FullScreenVideoViewModel(INavigationService navigationService,
                                        IErrorHandleService errorHandleService,
                                        IApiService apiService,
                                        IDialogService dialogService,
                                        ISettingsService settingsService,
                                        IPlatformService platformService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Interaction = new MvxInteraction();

            _platformService = platformService;
            ShareCommand = new MvxAsyncCommand(ShareAsync);
            MoveNextCommand = new MvxCommand(MoveNext);
            MovePreviousCommand = new MvxCommand(MovePrevious);
            OpenCommentsCommand = new MvxRestrictedAsyncCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        public IMvxAsyncCommand ShareCommand { get; }

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

        public bool IsLikeFlowAvailable { get; private set; }

        public long? NumberOfComments { get; private set; }

        public string NumberOfLikesPresentation => NumberOfLikes.ToCountString();

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

        protected override void OnLikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfLikesPresentation));
            _isReloadNeeded = true;
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
            var currentVideo = _videos.ElementAtOrDefault(_index);
            if (currentVideo is null)
            {
                return;
            }

            VideoId = currentVideo.VideoId;
       
            VideoName = currentVideo.VideoName;
            Description = currentVideo.Description;
            ProfilePhotoUrl = currentVideo.ProfilePhotoUrl;
            NumberOfLikes = currentVideo.NumberOfLikes;
            IsLikeFlowAvailable = currentVideo.IsLikeFlowAvailable;
            NumberOfComments = currentVideo.NumberOfComments;

            _shareLink = currentVideo.ShareLink;
            RaiseAllPropertiesChanged();

            IsLiked = currentVideo.IsLiked;
            VideoUrl = currentVideo.VideoUrl;
        }

        private Task ShareAsync()
        {
            return _platformService.ShareUrlAsync(Resources.ShareDialog_LinkShareTitle, _shareLink);
            //TODO: remove comments when logic will be ready
            //return DialogService.ShowShareDialogAsync(_shareLink);
        }
    }
}