using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Video
{
    public class FullScreenVideoViewModel : BaseViewModel, IMvxViewModel<string>
    {
        private string videoUrl;
        private bool isMuted;

        public FullScreenVideoViewModel(INavigationService navigationService,
                                        IErrorHandleService errorHandleService,
                                        IApiService apiService,
                                        IDialogService dialogService) : base(navigationService, errorHandleService, apiService, dialogService)
        {
        }

        public string VideoUrl
        {
            get => videoUrl;
            private set => SetProperty(ref videoUrl, value);
        }

        public bool IsMuted
        {
            get => isMuted;
            set => SetProperty(ref isMuted, value);
        }

        public void Prepare(string parameter)
        {
            VideoUrl = parameter;
        }
    }
}
