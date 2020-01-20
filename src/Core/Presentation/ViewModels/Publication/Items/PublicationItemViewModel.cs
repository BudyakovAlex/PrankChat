using System;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items
{
    public class PublicationItemViewModel : BasePublicationViewModel
    {
        public IVideoPlayerService VideoPlayerService { get; }

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
                    profileName,
                    profilePhotoUrl,
                    videoName,
                    videoUrl,
                    numberOfViews,
                    publicationDate,
                    numberOfLikes,
                    shareLink)
        {
            VideoPlayerService = videoPlayerService;
        }

        public MvxAsyncCommand ShowDetailsCommand => new MvxAsyncCommand(() => NavigationService.ShowDetailsPublicationView());

        public MvxCommand ToggleSound => new MvxCommand(OnToggleSoundCommand);

        private bool _soundMuted;
        public bool SoundMuted
        {
            get => _soundMuted;
            set => SetProperty(ref _soundMuted, value);
        }

        private void OnToggleSoundCommand()
        {
            SoundMuted = !SoundMuted;

            VideoPlayerService.Muted = SoundMuted;
        }
    }
}
