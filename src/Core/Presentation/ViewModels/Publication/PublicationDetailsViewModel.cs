﻿using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationDetailsViewModel : BasePublicationViewModel
    {
        private DateTime _commentDate = new DateTime(2018, 4, 24);
        private int? _numberOfComments = 125;

        public PublicationDetailsViewModel(IPublicationsManager publicationsManager, IVideoManager videoManager) : base(publicationsManager, videoManager)
        {
            OpenCommentsCommand = this.CreateRestrictedCommand(ShowCommentsAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        #region Video

        public string VideoDescription { get; set; } = "Description video one";

        #endregion

        #region Last Comment

        public string CommentatorName { get; } = "Name one";

        public string CommentatorPhotoUrl { get; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        public string CommentDateText => _commentDate.ToTimeAgoCommentString();

        public string NumberOfCommentText => _numberOfComments.ToCountString();

        #endregion

        public MvxRestrictedAsyncCommand OpenCommentsCommand { get; }

        private Task ShowCommentsAsync()
        {
            return NavigationManager.NavigateAsync<CommentsViewModel, int, int>(VideoId);
        }
    }
}