using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BasePublicationViewModel
    {
        public PublicationItemViewModel(INavigationService navigationService,
                                        IDialogService dialogService,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        string profileName,
                                        string profilePhotoUrl,
                                        string videoName,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes,
                                        string shareLink)
            : base(navigationService,
                    dialogService,
                    platformService,
                    videoPlayerService,
                    profileName,
                    profilePhotoUrl,
                    videoName,
                    videoUrl,
                    numberOfViews,
                    publicationDate,
                    numberOfLikes,
                    shareLink)
        {
        }

        public MvxAsyncCommand ShowDetailsCommand => new MvxAsyncCommand(() => NavigationService.ShowDetailsPublicationView());
    }
}
