﻿using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
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
        private readonly IPlatformService _platformService;
        private readonly VideoDataModel _videoDataModel;

        private readonly Func<List<FullScreenVideoDataModel>> _getAllFullScreenVideoDataFunc;
        private readonly string[] _restrictedActionsInDemoMode = new[]
        {
             Resources.Publication_Item_Complain,
             Resources.Publication_Item_Subscribe_To_Author
        };

        private MvxSubscriptionToken _updateNumberOfViewsSubscriptionToken;
        private long? _numberOfViews;
        private DateTime _publicationDate;
        private string _shareLink;

        //NOTE: stub for publication details page
        public BasePublicationViewModel()
        {
            ShowCommentsCommand = new MvxRestrictedAsyncCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        public BasePublicationViewModel(IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        VideoDataModel videoDataModel,
                                        Func<List<FullScreenVideoDataModel>> getAllFullScreenVideoDataFunc)
        {
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
            VideoPlaceholderImageUrl = _videoDataModel.Poster;
            NumberOfComments = videoDataModel.CommentsCount;

            _numberOfViews = videoDataModel.ViewsCount;
            _publicationDate = videoDataModel.CreatedAt.DateTime;
            _shareLink = videoDataModel.ShareUri;

            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;
            Subscribe();

            ShowCommentsCommand = new MvxRestrictedAsyncCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => SettingsService.User != null, handleFunc: NavigationService.ShowLoginView);
            BookmarkCommand = new MvxRestrictedAsyncCommand(BookmarkAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            ShowFullScreenVideoCommand = new MvxAsyncCommand(ShowFullScreenVideoAsync);
            ShareCommand = new MvxAsyncCommand(ShareAsync);
            OpenSettingsCommand = new MvxAsyncCommand(OpenSettingAsync);
            ToggleSoundCommand = new MvxCommand(ToggleSound);
        }

        #region Profile

        public string ProfileName { get; set; }

        public string ProfileShortName => ProfileName.ToShortenName();

        public string ProfilePhotoUrl { get; set; }

        public bool CanPlayVideo => true;

        #endregion Profile

        #region Video

        public string VideoInformationText => $"{_numberOfViews.ToCountViewsString()} • {_publicationDate.ToTimeAgoPublicationString()}";

        public string VideoName { get; set; }

        public string Description { get; }

        public string PlaceholderImageUrl { get; set; }

        public string VideoUrl { get; set; }

        public string PreviewUrl { get; set; }

        public long? NumberOfComments { get; private set; }

        public string NumberOfCommentsPresentation => NumberOfComments.ToCountString();

        public string VideoPlaceholderImageUrl { get; }

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

        //TODO: add correct logic
        public bool IsCompetiotionVideo => false;

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

        protected override void OnDislikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfDislikesText));
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscribe();

            base.ViewDestroy(viewFinishing);
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
                                                IsDisliked);
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

        private void Subscribe()
        {
            _updateNumberOfViewsSubscriptionToken = Messenger.Subscribe<ViewCountMessage>(viewCount =>
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

            Messenger?.Unsubscribe<ViewCountMessage>(_updateNumberOfViewsSubscriptionToken);
            _updateNumberOfViewsSubscriptionToken.Dispose();
            _updateNumberOfViewsSubscriptionToken = null;
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

                await ApiService.ComplainVideoAsync(VideoId, text, text);
                DialogService.ShowToast(Resources.Complaint_Complete_Message, ToastType.Positive);
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
