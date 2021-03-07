﻿using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract
{
    public abstract class LikeableViewModel : BasePageViewModel
    {
        private readonly IPublicationsManager _publicationsManager;

        private CancellationTokenSource _cancellationSendingLikeTokenSource;
        private CancellationTokenSource _cancellationSendingDislikeTokenSource;

        public LikeableViewModel(IPublicationsManager publicationsManager)
        {
            _publicationsManager = publicationsManager;

            LikeCommand = new MvxRestrictedCommand(
                OnLike,
                restrictedExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            DislikeCommand = new MvxRestrictedCommand(
                OnDislike,
                restrictedExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public IMvxCommand LikeCommand { get; }

        public IMvxCommand DislikeCommand { get; }

        public int VideoId { get; set; }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        private bool _isDisliked;
        public bool IsDisliked
        {
            get => _isDisliked;
            set => SetProperty(ref _isDisliked, value);
        }

        protected long? NumberOfLikes { get; set; }

        protected long? NumberOfDislikes { get; set; }

        protected virtual void OnLikeChanged()
        {
        }

        protected virtual void OnDislikeChanged()
        {
        }

        protected virtual void OnDislike()
        {
            IsDisliked = !IsDisliked;

            var totalDislikes = IsDisliked ? NumberOfDislikes + 1 : NumberOfDislikes - 1;
            NumberOfDislikes = totalDislikes > 0 ? totalDislikes : 0;

            OnDislikeChanged();
            SendDislikeAsync().FireAndForget();

            if (IsDisliked && IsLiked)
            {
                IsLiked = false;
                var totalLikes = NumberOfLikes - 1;
                NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

                OnLikeChanged();
            }
        }

        protected virtual void OnLike()
        {
            IsLiked = !IsLiked;

            var totalLikes = IsLiked ? NumberOfLikes + 1 : NumberOfLikes - 1;
            NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

            OnLikeChanged();
            SendLikeAsync().FireAndForget();

            if (IsLiked && IsDisliked)
            {
                IsDisliked = false;
                var totalDislikes = NumberOfDislikes - 1;
                NumberOfDislikes = totalDislikes > 0 ? totalDislikes : 0;

                OnDislikeChanged();
            }
        }

        private async Task SendLikeAsync()
        {
            _cancellationSendingLikeTokenSource?.Cancel();
            if (_cancellationSendingLikeTokenSource == null)
            {
                _cancellationSendingLikeTokenSource = new CancellationTokenSource();
            }

            try
            {
                var video = await _publicationsManager.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);

                NumberOfLikes = video.LikesCount;
                NumberOfDislikes = video.DislikesCount;
                IsLiked = video.IsLiked;
                IsDisliked = video.IsDisliked;
                OnDislikeChanged();
                OnLikeChanged();
            }
            catch
            {
                //TODO log this
            }
            finally
            {
                _cancellationSendingLikeTokenSource?.Dispose();
                _cancellationSendingLikeTokenSource = null;
            }
        }

        private async Task SendDislikeAsync()
        {
            _cancellationSendingDislikeTokenSource?.Cancel();
            if (_cancellationSendingDislikeTokenSource == null)
            {
                _cancellationSendingDislikeTokenSource = new CancellationTokenSource();
            }

            try
            {
                var video = await _publicationsManager.SendDislikeAsync(VideoId, IsDisliked, _cancellationSendingDislikeTokenSource.Token);

                NumberOfLikes = video.LikesCount;
                NumberOfDislikes = video.DislikesCount;
                IsLiked = video.IsLiked;
                IsDisliked = video.IsDisliked;
                OnDislikeChanged();
                OnLikeChanged();
            }
            catch
            {
                //TODO log this
            }
            finally
            {
                _cancellationSendingDislikeTokenSource?.Dispose();
                _cancellationSendingDislikeTokenSource = null;
            }
        }
    }
}