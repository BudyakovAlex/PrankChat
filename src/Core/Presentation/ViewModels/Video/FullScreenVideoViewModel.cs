using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Video
{
    public class FullScreenVideoViewModel : LikeableViewModel, IMvxViewModel<FullScreenVideoParameter>
    {
        private string _shareLink;

        public FullScreenVideoViewModel(INavigationService navigationService,
                                        IErrorHandleService errorHandleService,
                                        IApiService apiService,
                                        IDialogService dialogService,
                                        ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            ShareCommand = new MvxRestrictedAsyncCommand(ShareAsync, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            OpenCommentsCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowCommentsView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        public MvxRestrictedAsyncCommand ShareCommand { get; }

        public MvxRestrictedAsyncCommand OpenCommentsCommand { get; }

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            private set => SetProperty(ref _videoUrl, value);
        }

        public bool IsMuted
        {
            get => Xamarin.Essentials.Preferences.Get(Constants.Keys.MuteStateKey, false);
            set
            {
                Xamarin.Essentials.Preferences.Set(Constants.Keys.MuteStateKey, value);
                RaisePropertyChanged();
            }
        }

        public string VideoName { get; private set; }

        public string Description { get; private set; }

        public string ProfilePhotoUrl { get; private set; }

        public string NumberOfLikesPresentation => NumberOfLikes.ToCountString();

        public void Prepare(FullScreenVideoParameter parameter)
        {
            VideoId = parameter.VideoId;
            VideoUrl = parameter.VideoUrl;
            VideoName = parameter.VideoName;
            Description = parameter.Description;
            ProfilePhotoUrl = parameter.ProfilePhotoUrl;
            NumberOfLikes = parameter.NumberOfLikes;
            IsLiked = parameter.IsLiked;

            _shareLink = parameter.ShareLink;
        }

        protected override void OnLikeChanged()
        {
            RaisePropertyChanged(nameof(NumberOfLikesPresentation));
        }

        private Task ShareAsync()
        {
            return DialogService.ShowShareDialogAsync(_shareLink);
        }
    }
}
