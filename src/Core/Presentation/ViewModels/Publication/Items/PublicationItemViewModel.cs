using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BaseVideoItemViewModel, IDisposable
    {
        private readonly string[] _restrictedActionsInDemoMode = new[]
        {
             Resources.Publication_Item_Complain,
             Resources.Publication_Item_Subscribe_To_Author,
             Resources.Publication_Item_Download
        };

        private readonly Func<BaseVideoItemViewModel[]> _getAllFullScreenVideosFunc;
        private readonly IUsersManager _usersManager;

        private long? _numberOfViews;
        private readonly DateTime _publicationDate;

        public PublicationItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video,
            Func<BaseVideoItemViewModel[]> getAllFullScreenVideosFunc,
            IUsersManager usersManager) : base(videoManager, userSessionProvider, video)
        {
            _usersManager = usersManager;
            ProfileName = video.Customer?.Login;
            IsCompetitionVideo = video.OrderCategory.CheckIsCompetitionOrder();

            _numberOfViews = video.ViewsCount;
            _publicationDate = video.CreatedAt.DateTime;

            _getAllFullScreenVideosFunc = getAllFullScreenVideosFunc;

            Messenger.SubscribeWithCondition<ViewCountMessage>(msg => msg.VideoId == VideoId, OnViewCountChanged).DisposeWith(Disposables);

            ShowCommentsCommand = this.CreateRestrictedCommand(
                ShowCommentsAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
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

        public override bool CanPlayVideo => true;

        public override bool CanVoteVideo => true;

        public IMvxAsyncCommand BookmarkCommand { get; }

        public IMvxAsyncCommand ShowFullScreenVideoCommand { get; }

        public IMvxAsyncCommand ShowCommentsCommand { get; }

        //TODO: remove comments when all logic will be ready
        //public MvxAsyncCommand ShareCommand => new MvxAsyncCommand(() => DialogService.ShowShareDialogAsync(_shareLink));

        public IMvxAsyncCommand ShareCommand { get; }

        public IMvxAsyncCommand OpenSettingsCommand { get; }

        public IMvxCommand ToggleSoundCommand { get; }

        public string ProfileName { get; }

        public string VideoInformationText => $"{_numberOfViews.ToCountViewsString()} • {_publicationDate.ToTimeAgoPublicationString()}";

        public string PlaceholderImageUrl { get; }

        public string NumberOfCommentsPresentation => NumberOfComments.ToCountString();

        private bool _hasSoundTurnOn;
        public bool HasSoundTurnOn
        {
            get => _hasSoundTurnOn;
            set => SetProperty(ref _hasSoundTurnOn, value);
        }

        public string NumberOfLikesText => NumberOfLikes.ToCountString();

        public string NumberOfDislikesText => NumberOfDislikes.ToCountString();

        public bool IsCompetitionVideo { get; }

        protected override User User => Video.Customer;

        protected override void OnLikeChanged() =>
            RaisePropertyChanged(nameof(NumberOfLikesText));

        protected override void OnDislikeChanged() =>
            RaisePropertyChanged(nameof(NumberOfDislikesText));

        private void OnViewCountChanged(ViewCountMessage viewCountMessage)
        {
            _numberOfViews = viewCountMessage.ViewsCount;
            RaisePropertyChanged(nameof(VideoInformationText));
        }

        private Task ShareAsync() => Share.RequestAsync(new ShareTextRequest
        {
            Uri = ShareLink,
            Title = Resources.ShareDialog_LinkShareTitle
        });

        private async Task ShowFullScreenVideoAsync()
        {
            var items = _getAllFullScreenVideosFunc?.Invoke() ?? new [] { this };
            var currentItem = items.FirstOrDefault(item => item.VideoId == VideoId);
            var index = currentItem is null ? 0 : items.IndexOfOrDefault(currentItem);
            var navigationParams = new FullScreenVideoParameter(items, index);
            if (navigationParams.Videos.Length == 0)
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
                Resources.Block_User,
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
                await Clipboard.SetTextAsync(ShareLink);
                DialogService.ShowToast(Resources.LinkCopied, ToastType.Positive);
                return;
            }

            if (result == Resources.Publication_Item_Download)
            {
                _ = DownloadVideoAsync();
            }

            if (result == Resources.Block_User)
            {
                await BlockUserAsync();
            }
        }

        private async Task BlockUserAsync()
        {
            var isComplaintSent = await _usersManager.ComplainUserAsync(UserId, string.Empty, string.Empty);
            if (!isComplaintSent)
            {
                DialogService.ShowToast(Resources.Error_Something_Went_Wrong_Message, ToastType.Negative);
                return;
            }

            var message = string.Format(Resources.Blocked_User, User.Login);
            DialogService.ShowToast(message, ToastType.Positive);
            Messenger.Publish(new ReloadPublicationsMessage(this));
        }

        private Task DownloadVideoAsync()
        {
            var videoFileName = Video.Title.ReplaceSpacesWithUnderscores().ToLower();
            return VideoManager.DownloadVideoAsync(Video.MarkedStreamUri, videoFileName);
        }

        private async Task ComplaintAsync()
        {
            var text = await DialogService.ShowMenuDialogAsync(Constants.ComplaintConstants.CommonCompetitionAims);
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            await VideoManager.ComplainVideoAsync(VideoId, text, text);
            DialogService.ShowToast(Resources.Complaint_Complete_Message, ToastType.Positive);
            Messenger.Publish(new ReloadPublicationsMessage(this));
        }

        private void ToggleSound()
        {
            HasSoundTurnOn = !HasSoundTurnOn;
            PreviewVideoPlayer.IsMuted = !HasSoundTurnOn;
        }
    }
}