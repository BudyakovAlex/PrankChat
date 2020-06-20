using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationDetailsViewModel : BasePublicationViewModel
    {
        private DateTime _commentDate = new DateTime(2018, 4, 24);
        private int? _numberOfComments = 125;

        #region Video

        public string VideoDescription { get; set; } = "Description video one";

        #endregion

        #region Last Comment

        public string CommentatorName { get; } = "Name one";

        public string CommentatorPhotoUrl { get; } = "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500";

        public string CommentDateText => _commentDate.ToTimeAgoCommentString();

        public string NumberOfCommentText => _numberOfComments.ToCountString();

        #endregion

        public MvxRestrictedAsyncCommand OpenCommentsCommand => new MvxRestrictedAsyncCommand(() => NavigationService.ShowCommentsView(VideoId), restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);

        public PublicationDetailsViewModel(INavigationService navigationService,
                                           IErrorHandleService errorHandleService,
                                           IApiService apiService,
                                           IDialogService dialogService,
                                           ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }
    }
}
