using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class BasePublicationViewModel : BaseViewModel
    {
        private const int RegistrationDelayInMilliseconds = 3000;
        private readonly IPlatformService _platformService;
        private readonly IMvxMessenger _mvxMessenger;

        private MvxSubscriptionToken _updateNumberOfViewsSubscriptionToken;
        private long? _numberOfViews;
        private DateTime _publicationDate;
        private long? _numberOfLikes;
        private string _shareLink;
        private CancellationTokenSource _cancellationSendingLikeTokenSource;

        #region Profile

        public string ProfileName { get; set; }

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl { get; set; }

        #endregion Profile

        #region Video

        public string VideoInformationText => $"{_numberOfViews.ToCountViewsString()} • {_publicationDate.ToTimeAgoPublicationString()}";

        public int VideoId { get; set; }

        public string VideoName { get; set; }

        public string PlaceholderImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public IVideoPlayerService VideoPlayerService { get; }

        private bool _hasSoundTurnOn;

        public bool HasSoundTurnOn
        {
            get => _hasSoundTurnOn;
            set => SetProperty(ref _hasSoundTurnOn, value);
        }

        private bool _isLiked;

        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        #endregion Video

        public string NumberOfLikesText => $"{Resources.Like} {_numberOfLikes.ToCountString()}";

        #region Commands

        public MvxCommand LikeCommand => new MvxCommand(OnLike);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(() => DialogService.ShowShareDialogAsync(_shareLink));

        public MvxAsyncCommand BookmarkCommand => new MvxAsyncCommand(OnBookmarkAsync);

        public MvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OnOpenSettingAsync);

        public MvxCommand ToggleSoundCommand => new MvxCommand(OnToggleSound);

        public MvxAsyncCommand ShowFullScreenVideoCommand => new MvxAsyncCommand(ShowFullScreenVideoAsync);

        #endregion Commands

        public BasePublicationViewModel(INavigationService navigationService,
                                        IErrorHandleService errorHandleService,
                                        IApiService apiService,
                                        IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
        }

        public BasePublicationViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        IApiService apiService,
                                        IErrorHandleService errorHandleService,
                                        IMvxMessenger mvxMessenger,
                                        string profileName,
                                        string profilePhotoUrl,
                                        int videoId,
                                        string videoName,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes,
                                        string shareLink,
                                        bool isLiked)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _platformService = platformService;
            _mvxMessenger = mvxMessenger;

            VideoPlayerService = videoPlayerService;
            ProfileName = profileName;
            ProfilePhotoUrl = profilePhotoUrl;
            VideoId = videoId;
            VideoName = videoName;
            VideoUrl = videoUrl;
            IsLiked = isLiked;

            _numberOfViews = numberOfViews;
            _publicationDate = publicationDate;
            _numberOfLikes = numberOfLikes;
            _shareLink = shareLink;

            Subscribe();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscribe();

            base.ViewDestroy(viewFinishing);
        }

        private void Subscribe()
        {
            _updateNumberOfViewsSubscriptionToken = _mvxMessenger.Subscribe<ViewCountMessage>(viewCount =>
            {
                if (viewCount.VideoId == VideoId)
                {
                    _numberOfViews = viewCount.ViewsCount;
                    RaisePropertyChanged(nameof(VideoInformationText));
                }
            });
        }

        private void Unsubscribe()
        {
            _mvxMessenger.Unsubscribe<ViewCountMessage>(_updateNumberOfViewsSubscriptionToken);
            _updateNumberOfViewsSubscriptionToken.Dispose();
            _updateNumberOfViewsSubscriptionToken = null;
        }

        private Task ShowFullScreenVideoAsync()
        {
            VideoPlayerService.Player.TryRegisterViewedFact(VideoId, RegistrationDelayInMilliseconds);
            return NavigationService.ShowFullScreenVideoView(VideoUrl);
        }

        private void OnLike()
        {
            IsLiked = !IsLiked;
            _numberOfLikes = IsLiked
                            ? _numberOfLikes + 1
                            : _numberOfLikes - 1;
            RaisePropertyChanged(nameof(NumberOfLikesText));
            SendLike().FireAndForget();
        }

        private async Task SendLike()
        {
            _cancellationSendingLikeTokenSource?.Cancel();
            if (_cancellationSendingLikeTokenSource == null)
            {
                _cancellationSendingLikeTokenSource = new CancellationTokenSource();
            }

            try
            {
                await ApiService.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);
            }
            finally
            {
                _cancellationSendingLikeTokenSource?.Dispose();
                _cancellationSendingLikeTokenSource = null;
            }
        }

        private Task OnBookmarkAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OnOpenSettingAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.Publication_Item_Complain,
                Resources.Publication_Item_Copy_Link,
                Resources.Publication_Item_Subscribe_To_Author
            });

            if (string.IsNullOrWhiteSpace(result))
                return;

            if (result == Resources.Publication_Item_Complain)
            {
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await _platformService.CopyTextAsync(_shareLink);
                return;
            }

            if (result == Resources.Publication_Item_Subscribe_To_Author)
            {
                return;
            }
        }

        private void OnToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;

            VideoPlayerService.Muted = !HasSoundTurnOn;
        }
    }
}
