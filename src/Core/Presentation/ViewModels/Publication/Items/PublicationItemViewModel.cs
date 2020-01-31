using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BasePublicationViewModel
    {
        public MvxAsyncCommand ShowDetailsCommand => new MvxAsyncCommand(NavigationService.ShowDetailsPublicationView);

        public PublicationItemViewModel(INavigationService navigationService,
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
            : base(navigationService,
                    dialogService,
                    platformService,
                    videoPlayerService,
                    apiServices,
                    errorHandleService,
                    profileName,
                    profilePhotoUrl,
                    videoId,
                    videoName,
                    videoUrl,
                    numberOfViews,
                    publicationDate,
                    numberOfLikes,
                    shareLink,
                    isLiked)
        {
        }
    }
}
