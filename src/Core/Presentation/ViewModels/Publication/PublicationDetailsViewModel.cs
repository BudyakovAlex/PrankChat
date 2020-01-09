﻿using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationDetailsViewModel : BasePublicationViewModel
    {
        private DateTime _commentDate = new DateTime(2018, 4, 24);
        private int _numberOfComments = 125;

        #region Video

        public string VideoDescription { get; set; } = "Description video one";

        #endregion

        #region Last Comment

        public string CommentatorName { get; } = "Name one";

        public string CommentatorPhotoUrl { get; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        public string CommentDateText => $"{_commentDate}";

        public string NumberOfCommentText => $"{_numberOfComments}";

        #endregion

        public MvxAsyncCommand OpenCommentsCommand => new MvxAsyncCommand(() => NavigationService.ShowCommentsView());

        public PublicationDetailsViewModel(INavigationService navigationService, IDialogService dialogService)
            : base(navigationService, dialogService)
        {
        }
    }
}
