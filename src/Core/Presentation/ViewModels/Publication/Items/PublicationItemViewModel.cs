using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BasePublicationViewModel
    {
        public IMvxAsyncCommand ShowDetailsCommand { get; }

        public PublicationItemViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        IApiService apiServices,
                                        IErrorHandleService errorHandleService,
                                        IMvxMessenger mvxMessenger,
                                        ISettingsService settingsService,
                                        string poster,
                                        string profileName,
                                        string profilePhotoUrl,
                                        int videoId,
                                        string videoName,
                                        string videoDescription,
                                        string videoUrl,
                                        string previewUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes,
                                        long numberOfComments,
                                        string shareLink,
                                        bool isLiked,
                                        Func<List<FullScreenVideoDataModel>> getAllFullScreenVideoDataFunc)
            : base(navigationService,
                   dialogService,
                   platformService,
                   videoPlayerService,
                   apiServices,
                   errorHandleService,
                   mvxMessenger,
                   settingsService,
                   poster,
                   profileName,
                   profilePhotoUrl,
                   videoId,
                   videoName,
                   videoDescription,
                   videoUrl,
                   previewUrl,
                   numberOfViews,
                   publicationDate,
                   numberOfLikes,
                   numberOfComments,
                   shareLink,
                   isLiked,
                   getAllFullScreenVideoDataFunc)
        {
            // TODO: Unblock this after video details page will be completed
            // ShowDetailsCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowDetailsPublicationView, restrictedCanExecute: ()=> IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }
    }
}
