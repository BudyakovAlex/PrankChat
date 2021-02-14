using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BasePublicationViewModel
    {
        public IMvxAsyncCommand ShowDetailsCommand { get; }

        public PublicationItemViewModel(IPublicationsManager publicationsManager,
                                        IVideoManager videoManager,
                                        IPlatformService platformService,
                                        IVideoPlayerService videoPlayerService,
                                        Models.Data.Video videoDataModel,
                                        Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc)
            : base(publicationsManager,
                   videoManager,
                   platformService,
                   videoPlayerService,
                   videoDataModel,
                   getAllFullScreenVideoDataFunc)
        {
            // TODO: Unblock this after video details page will be completed
            // ShowDetailsCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowDetailsPublicationView, restrictedCanExecute: ()=> IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }
    }
}
