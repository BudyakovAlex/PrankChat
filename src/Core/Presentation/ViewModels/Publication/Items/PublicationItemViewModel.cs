using MvvmCross.Commands;
using Plugin.DownloadManager;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : LikeableViewModel, IVideoItemViewModel, IDisposable
    {
        private readonly IVideoManager _videoManager;

        private readonly Models.Data.Video _video;

        private readonly Func<List<FullScreenVideo>> _getAllFullScreenVideoDataFunc;

        private readonly string[] _restrictedActionsInDemoMode = new[]
        {
             Resources.Publication_Item_Complain,
             Resources.Publication_Item_Subscribe_To_Author,
             Resources.Publication_Item_Download
        };

        private long? _numberOfViews;
        private readonly DateTime _publicationDate;
        private readonly string _shareLink;

        public PublicationItemViewModel(
            IPublicationsManager publicationsManager,
            IVideoManager videoManager,
            Models.Data.Video video,
            Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc) : base(publicationsManager)
        {
            _videoManager = videoManager;
            _video = video;

            ProfileName = _video.Customer?.Login;
            ProfilePhotoUrl = _video.Customer?.Avatar;
            VideoId = _video.Id;
            VideoName = _video.Title;
            Description = _video.Description;
            VideoUrl = _video.StreamUri;
            PreviewUrl = video.PreviewUri;
            IsLiked = video.IsLiked;
            IsDisliked = video.IsDisliked;
            NumberOfLikes = video.LikesCount;
            NumberOfDislikes = video.DislikesCount;
            StubImageUrl = _video.Poster;
            NumberOfComments = video.CommentsCount;
            IsCompetitionVideo = video.OrderCategory.CheckIsCompetitionOrder();

            _numberOfViews = video.ViewsCount;
            _publicationDate = video.CreatedAt.DateTime;
            _shareLink = video.ShareUri;

            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            Messenger.SubscribeWithCondition<ViewCountMessage>(msg => msg.VideoId == VideoId, OnViewCountChanged).DisposeWith(Disposables);

            ShowCommentsCommand = this.CreateRestrictedCommand(
                ShowCommentsAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            OpenUserProfileCommand = this.CreateRestrictedCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => UserSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            BookmarkCommand = this.CreateRestrictedCommand(
                BookmarkAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            ShowFullScreenVideoCommand = this.CreateCommand(ShowFullScreenVideoAsync);
            ShareCommand = this.CreateCommand(ShareAsync);
            OpenSettingsCommand = this.CreateCommand(OpenSettingAsync);
            ToggleSoundCommand = this.CreateCommand(ToggleSound);
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

        protected override void OnLikeChanged() =>
            RaisePropertyChanged(nameof(NumberOfLikesText));

        protected override void OnDislikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfDislikesText));
        }

        public FullScreenVideo GetFullScreenVideo()
        {
            return new FullScreenVideo(
                _video.Customer?.Id ?? 0,
                _video.Customer?.IsSubscribed ?? false,
                VideoId,
                VideoUrl,
                VideoName,
                Description,
                _shareLink,
                ProfilePhotoUrl,
                _video.Customer?.Login?.ToShortenName(),
                NumberOfLikes,
                NumberOfDislikes,
                NumberOfComments,
                IsLiked,
                IsDisliked,
                StubImageUrl);
        }

        private void OnViewCountChanged(ViewCountMessage viewCountMessage)
        {
            _numberOfViews = viewCountMessage.ViewsCount;
            RaisePropertyChanged(nameof(VideoInformationText));
        }

        private Task OpenUserProfileAsync()
        {
            if (_video.Customer?.Id is null ||
                _video.Customer.Id == UserSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_video.Customer.Id);
        }

        private Task ShareAsync() => Share.RequestAsync(new ShareTextRequest
        {
            Uri = _shareLink,
            Title = Resources.ShareDialog_LinkShareTitle
        });

        private async Task ShowFullScreenVideoAsync()
        {
            var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideo> { GetFullScreenVideo() };
            var currentItem = items.FirstOrDefault(item => item.VideoId == VideoId);
            var index = currentItem is null ? 0 : items.IndexOf(currentItem);
            var navigationParams = new FullScreenVideoParameter(items, index);
            if (navigationParams.Videos.Count == 0)
            {
                return;
            }

            var shouldRefresh = await NavigationManager.NavigateAsync<FullScreenVideoViewModel, FullScreenVideoParameter, bool>(navigationParams);
            if (!shouldRefresh)
            {
                return;
            }

            Messenger.Publish(new ReloadPublicationsMessage(this));
        }

        private async Task ShowCommentsAsync()
        {
            var commentsCount = await NavigationManager.NavigateAsync<CommentsViewModel, int, int>(VideoId);
            NumberOfComments = commentsCount > 0 ? commentsCount : NumberOfComments;
            await RaisePropertyChanged(nameof(NumberOfCommentsPresentation));
        }

        private Task BookmarkAsync()
        {
            //TODO: add bookmark logic here
            return Task.CompletedTask;
        }

        private async Task OpenSettingAsync()
        {
            var result = await DialogService.ShowMenuDialogAsync(new string[]
            {
                Resources.Publication_Item_Complain,
                Resources.Publication_Item_Copy_Link,
                Resources.Publication_Item_Download,
            });

            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            if (!IsUserSessionInitialized && _restrictedActionsInDemoMode.Contains(result))
            {
                await NavigationManager.NavigateAsync<LoginViewModel>();
                return;
            }

            if (result == Resources.Publication_Item_Complain)
            {
                await ComplaintAsync();
                return;
            }

            if (result == Resources.Publication_Item_Copy_Link)
            {
                await Clipboard.SetTextAsync(_shareLink);
                DialogService.ShowToast(Resources.LinkCopied, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Download)
            {
                DownloadVideo();
            }
        }

        private void DownloadVideo()
        {
            var downloadManager = CrossDownloadManager.Current;
            var file = downloadManager.CreateDownloadFile(_video.MarkedStreamUri);
            downloadManager.Start(file);
        }

        private async Task ComplaintAsync()
        {
            var text = await DialogService.ShowMenuDialogAsync(Constants.ComplaintConstants.CommonCompetitionAims);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            await _videoManager.ComplainVideoAsync(VideoId, text, text);
            DialogService.ShowToast(Resources.Complaint_Complete_Message, ToastType.Positive);
        }

        private void ToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;

            VideoPlayerService.Muted = !HasSoundTurnOn;
        }
    }
}
