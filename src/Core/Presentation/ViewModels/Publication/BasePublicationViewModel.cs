using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.Exceptions;
using System.Threading;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class BasePublicationViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IPlatformService _platformService;
        private readonly IApiService _apiService;
        private readonly IErrorHandleService _errorHandleService;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private long? _numberOfViews;
        private DateTime _publicationDate;
        private long? _numberOfLikes;
        private string _shareLink;

        #region Profile

        public string ProfileName { get; set; }

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl { get; set; }

        #endregion

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

        #endregion

        public string NumberOfLikesText => $"{Resources.Like} {_numberOfLikes.ToCountString()}";

        #region Commands

        public MvxAsyncCommand LikeCommand => new MvxAsyncCommand(OnLikeAsync);

        public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(() => _dialogService.ShowShareDialogAsync(_shareLink));

        public MvxAsyncCommand BookmarkCommand => new MvxAsyncCommand(OnBookmarkAsync);

        public MvxAsyncCommand OpenSettingsCommand => new MvxAsyncCommand(OnOpenSettingAsync);

        public MvxCommand ToggleSoundCommand => new MvxCommand(OnToggleSound);

        #endregion

        public BasePublicationViewModel(INavigationService navigationService, IDialogService dialogService)
            : base(navigationService)
        {
            _dialogService = dialogService;
        }

        public BasePublicationViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        IApiService apiServices,
                                        IErrorHandleService errorHandleService,
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
            : base (navigationService)
        {
            _dialogService = dialogService;
            _platformService = platformService;
            _apiService = apiServices;
            _errorHandleService = errorHandleService;

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
        }

        private async Task OnLikeAsync()
        {
            await _semaphoreSlim.WaitAsync(0);
            try
            {
                IsLiked = !IsLiked;
                var video = await _apiService.SendLikeAsync(VideoId, IsLiked);
                if (video != null)
                {
                    _numberOfLikes = IsLiked
                        ? _numberOfLikes + 1
                        : _numberOfLikes - 1;
                    await RaisePropertyChanged(nameof(NumberOfLikesText));
                }
            }
            catch
            {
                IsLiked = !IsLiked;
                _errorHandleService.HandleException(new UserVisibleException("Произошла ошибка."));
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private Task OnBookmarkAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OnOpenSettingAsync()
        {
            var result = await _dialogService.ShowMenuDialogAsync(new string[]
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
