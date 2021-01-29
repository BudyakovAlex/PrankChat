using MvvmCross.Commands;
using Plugin.DownloadManager;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class BasePublicationViewModel : LikeableViewModel, IVideoItemViewModel, IDisposable
    {
        private readonly IVideoManager _videoManager;
        private readonly IPlatformService _platformService;

        private readonly VideoDataModel _videoDataModel;

        private readonly Func<List<FullScreenVideoDataModel>> _getAllFullScreenVideoDataFunc;
        private readonly string[] _restrictedActionsInDemoMode = new[]
        {
             Resources.Publication_Item_Complain,
             Resources.Publication_Item_Subscribe_To_Author,
             Resources.Publication_Item_Download
        };

        private long? _numberOfViews;
        private DateTime _publicationDate;
        private string _shareLink;

        //NOTE: stub for publication details page
        public BasePublicationViewModel(IPublicationsManager publicationsManager, IVideoManager videoManager) : base(publicationsManager)
        {
            _videoManager = videoManager;
            ShowCommentsCommand = new MvxRestrictedAsyncCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        public BasePublicationViewModel(IPublicationsManager publicationsManager,
                                        IVideoManager videoManager,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        VideoDataModel videoDataModel,
                                        Func<List<FullScreenVideoDataModel>> getAllFullScreenVideoDataFunc) : base(publicationsManager)
        {
            _videoManager = videoManager;
            _platformService = platformService;
            _videoDataModel = videoDataModel;

            VideoPlayerService = videoPlayerService;
            ProfileName = _videoDataModel.Customer?.Login;
            ProfilePhotoUrl = _videoDataModel.Customer?.Avatar;
            VideoId = _videoDataModel.Id;
            VideoName = _videoDataModel.Title;
            Description = _videoDataModel.Description;
            VideoUrl = _videoDataModel.StreamUri;
            PreviewUrl = videoDataModel.PreviewUri;
            IsLiked = videoDataModel.IsLiked;
            IsDisliked = videoDataModel.IsDisliked;
            NumberOfLikes = videoDataModel.LikesCount;
            NumberOfDislikes = videoDataModel.DislikesCount;
            StubImageUrl = _videoDataModel.Poster;
            NumberOfComments = videoDataModel.CommentsCount;
            IsCompetitionVideo = videoDataModel.OrderCategory.CheckIsCompetitionOrder();

            _numberOfViews = videoDataModel.ViewsCount;
            _publicationDate = videoDataModel.CreatedAt.DateTime;
            _shareLink = videoDataModel.ShareUri;

            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            Messenger.Subscribe<ViewCountMessage>(OnViewCountChanged).DisposeWith(Disposables);

            ShowCommentsCommand = new MvxRestrictedAsyncCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => SettingsService.User != null, handleFunc: NavigationService.ShowLoginView);
            BookmarkCommand = new MvxRestrictedAsyncCommand(BookmarkAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            ShowFullScreenVideoCommand = new MvxAsyncCommand(ShowFullScreenVideoAsync);
            ShareCommand = new MvxAsyncCommand(ShareAsync);
            OpenSettingsCommand = new MvxAsyncCommand(OpenSettingAsync);
            ToggleSoundCommand = new MvxCommand(ToggleSound);
        }

        #region Profile

        public string ProfileName { get; }

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl { get; }

        public bool CanPlayVideo => true;

        #endregion Profile

        #region Video

        public string VideoInformationText => $"{_numberOfViews.ToCountViewsString()} • {_publicationDate.ToTimeAgoPublicationString()}";

        public string VideoName { get; }

        public string Description { get; }

        public string PlaceholderImageUrl { get; }

        public string VideoUrl { get; }

        public string PreviewUrl { get; }

        public long? NumberOfComments { get; private set; }

        public string NumberOfCommentsPresentation => NumberOfComments.ToCountString();

        public string StubImageUrl { get; }

        public IVideoPlayerService VideoPlayerService { get; }

        private bool _hasSoundTurnOn;
        public bool HasSoundTurnOn
        {
            get => _hasSoundTurnOn;
            set => SetProperty(ref _hasSoundTurnOn, value);
        }

        public bool IsVideoProcessing => string.IsNullOrEmpty(VideoUrl);

        #endregion Video

        public string NumberOfLikesText => NumberOfLikes.ToCountString();

        public string NumberOfDislikesText => NumberOfDislikes.ToCountString();

        public bool IsCompetitionVideo { get; }

        #region Commands

        public IMvxAsyncCommand BookmarkCommand { get; }

        public IMvxAsyncCommand ShowFullScreenVideoCommand { get; }

        public IMvxAsyncCommand ShowCommentsCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        //TODO: remove comments when all logic will be ready
        //public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(() => DialogService.ShowShareDialogAsync(_shareLink));

        public IMvxAsyncCommand ShareCommand { get; }

        public IMvxAsyncCommand OpenSettingsCommand { get; }

        public IMvxCommand ToggleSoundCommand { get; }

        #endregion Commands

        protected override void OnLikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfLikesText));
        }

        protected override void OnDislikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfDislikesText));
        }

        public FullScreenVideoDataModel GetFullScreenVideoDataModel()
        {
            return new FullScreenVideoDataModel(_videoDataModel.Customer?.Id ?? 0,
                                                _videoDataModel.Customer?.IsSubscribed ?? false,
                                                VideoId,
                                                VideoUrl,
                                                VideoName,
                                                Description,
                                                _shareLink,
                                                ProfilePhotoUrl,
                                                _videoDataModel.Customer?.Login?.ToShortenName(),
                                                NumberOfLikes,
                                                NumberOfDislikes,
                                                NumberOfComments,
                                                IsLiked,
                                                IsDisliked,
                                                StubImageUrl);
        }

        private void OnViewCountChanged(ViewCountMessage viewCountMessage)
        {
            if (viewCountMessage.VideoId == VideoId)
            {
                _numberOfViews = viewCountMessage.ViewsCount;
                RaisePropertyChanged(nameof(VideoInformationText));
            }
        }

        private Task OpenUserProfileAsync()
        {
            if (_videoDataModel.Customer?.Id is null ||
                _videoDataModel.Customer.Id == SettingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return NavigationService.ShowUserProfile(_videoDataModel.Customer.Id);
        }

        private Task ShareAsync()
        {
            return _platformService.ShareUrlAsync(Resources.ShareDialog_LinkShareTitle, _shareLink);
        }

        private async Task ShowFullScreenVideoAsync()
        {
            VideoPlayerService.Player.TryRegisterViewedFact(VideoId, Constants.Delays.ViewedFactRegistrationDelayInMilliseconds);

            var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideoDataModel> { GetFullScreenVideoDataModel() };
            var currentItem = items.FirstOrDefault(item => item.VideoId == VideoId);
            var index = currentItem is null ? 0 : items.IndexOf(currentItem);
            var navigationParams = new FullScreenVideoParameter(items, index);
            var shouldRefresh = await NavigationService.ShowFullScreenVideoView(navigationParams);
            if (!shouldRefresh)
            {
                return;
            }

            Messenger.Publish(new ReloadPublicationsMessage(this));
        }

        private async Task ShowCommentsAsync()
        {
            var commentsCount = await NavigationService.ShowCommentsView(VideoId);
            NumberOfComments = commentsCount > 0 ? commentsCount : NumberOfComments;
            await RaisePropertyChanged(nameof(NumberOfCommentsPresentation));
        }

        private Task BookmarkAsync()
        {
            return Task.CompletedTask;
        }

        private async Task OpenSettingAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.Publication_Item_Complain,
                Resources.Publication_Item_Copy_Link,
                Resources.Publication_Item_Download,
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
                var text = await GetComplaintTextAsync();
                if (string.IsNullOrWhiteSpace(text))
                {
                    return;
                }

                await _videoManager.ComplainVideoAsync(VideoId, text, text);
                DialogService.ShowToast(Resources.Complaint_Complete_Message, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await _platformService.CopyTextAsync(_shareLink);
                DialogService.ShowToast(Resources.LinkCopied, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Download)
            {
                var downloadManager = CrossDownloadManager.Current;
                var file = downloadManager.CreateDownloadFile(_videoDataModel.MarkedStreamUri);
                downloadManager.Start(file);
            }

            // TODO: Subscription functionality not implemented.
            //if (result == Resources.Publication_Item_Subscribe_To_Author)
            //{
            //    return;
            //}
        }

        private async Task<string> GetComplaintTextAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(Constants.ComplaintConstants.CommonCompetitionAims);
            return result;
        }

        private void ToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;

            VideoPlayerService.Muted = !HasSoundTurnOn;
        }
    }
}
