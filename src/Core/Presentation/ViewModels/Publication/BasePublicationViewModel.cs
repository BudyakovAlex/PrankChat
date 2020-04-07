using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class BasePublicationViewModel : LikeableViewModel, IVideoItemViewModel, IDisposable
    {
        private readonly IPlatformService _platformService;
        private readonly IMvxMessenger _mvxMessenger;

        private readonly string[] _restrictedActionsInDemoMode = new[]
        {
             Resources.Publication_Item_Complain,
             Resources.Publication_Item_Subscribe_To_Author
        };

        private MvxSubscriptionToken _updateNumberOfViewsSubscriptionToken;
        private long? _numberOfViews;
        private DateTime _publicationDate;
        private string _shareLink;

        #region Profile

        public string ProfileName { get; set; }

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl { get; set; }

        #endregion Profile

        #region Video

        public string VideoInformationText => $"{_numberOfViews.ToCountViewsString()} • {_publicationDate.ToTimeAgoPublicationString()}";

        public string VideoName { get; set; }

        public string Description { get; }

        public string PlaceholderImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string VideoPlaceholderImageUrl { get; }

        public IVideoPlayerService VideoPlayerService { get; }

        private bool _hasSoundTurnOn;
        public bool HasSoundTurnOn
        {
            get => _hasSoundTurnOn;
            set => SetProperty(ref _hasSoundTurnOn, value);
        }

        #endregion Video

        public string NumberOfLikesText => NumberOfLikes.ToCountString();

        #region Commands

        public IMvxAsyncCommand BookmarkCommand => new MvxRestrictedAsyncCommand(OnBookmarkAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);

        public IMvxAsyncCommand ShowFullScreenVideoCommand => new MvxRestrictedAsyncCommand(ShowFullScreenVideoAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(() => DialogService.ShowShareDialogAsync(_shareLink));

        public MvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OnOpenSettingAsync);

        public MvxCommand ToggleSoundCommand => new MvxCommand(OnToggleSound);

        #endregion Commands

        public BasePublicationViewModel(INavigationService navigationService,
                                        IErrorHandleService errorHandleService,
                                        IApiService apiService,
                                        IDialogService dialogService,
                                        ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public BasePublicationViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        IApiService apiService,
                                        IErrorHandleService errorHandleService,
                                        IMvxMessenger mvxMessenger,
                                        ISettingsService settingsService,
                                        string poster,
                                        string profileName,
                                        string profilePhotoUrl,
                                        int videoId,
                                        string videoName,
                                        string description,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes,
                                        string shareLink,
                                        bool isLiked)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _platformService = platformService;
            _mvxMessenger = mvxMessenger;

            VideoPlayerService = videoPlayerService;
            ProfileName = profileName;
            ProfilePhotoUrl = profilePhotoUrl;
            VideoId = videoId;
            VideoName = videoName;
            Description = description;
            VideoUrl = videoUrl;
            IsLiked = isLiked;
            NumberOfLikes = numberOfLikes;
            VideoPlaceholderImageUrl = poster;

            _numberOfViews = numberOfViews;
            _publicationDate = publicationDate;
            _shareLink = shareLink;

            Subscribe();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unsubscribe();
            }
        }

        protected override void OnLikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfLikesText));
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
            if (_updateNumberOfViewsSubscriptionToken is null)
            {
                return;
            }

            _mvxMessenger?.Unsubscribe<ViewCountMessage>(_updateNumberOfViewsSubscriptionToken);
            _updateNumberOfViewsSubscriptionToken.Dispose();
            _updateNumberOfViewsSubscriptionToken = null;
        }

        private Task ShowFullScreenVideoAsync()
        {
            VideoPlayerService.Player.TryRegisterViewedFact(VideoId, Constants.Delays.ViewedFactRegistrationDelayInMilliseconds);

            var navigationParams = new FullScreenVideoParameter(VideoId,
                                                                VideoUrl,
                                                                VideoName,
                                                                Description,
                                                                _shareLink,
                                                                ProfilePhotoUrl,
                                                                NumberOfLikes,
                                                                IsLiked);
            return NavigationService.ShowFullScreenVideoView(navigationParams);
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
                // TODO: Subscription functionality not implemented.
                //Resources.Publication_Item_Subscribe_To_Author
            });

            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (!IsUserSessionInitialized && _restrictedActionsInDemoMode.Contains(result))
            {
                await NavigationService.ShowLoginView();
                return;
            }

            if (result == Resources.Publication_Item_Complain)
            {
                await ApiService.ComplainVideoAsync(VideoId, "n/a", "n/a");
                DialogService.ShowToast(Resources.ComplainSuccessful, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await _platformService.CopyTextAsync(_shareLink);
                DialogService.ShowToast(Resources.LinkCopied, ToastType.Positive);
                return;
            }

            // TODO: Subscription functionality not implemented.
            //if (result == Resources.Publication_Item_Subscribe_To_Author)
            //{
            //    return;
            //}
        }

        private void OnToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;

            VideoPlayerService.Muted = !HasSoundTurnOn;
        }
    }
}
