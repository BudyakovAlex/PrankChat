using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BasePublicationViewModel
    {
        public PublicationItemViewModel(INavigationService navigationService,
                                        string profileName,
                                        string profilePhotoUrl,
                                        string videoName,
                                        string videoUrl,
                                        long numberOfViews,
                                        DateTime publicationDate,
                                        long numberOfLikes)
            : base(navigationService,
                    profileName,
                    profilePhotoUrl,
                    videoName,
                    videoUrl,
                    numberOfViews,
                    publicationDate,
                    numberOfLikes)
        {
        }
    }
}
