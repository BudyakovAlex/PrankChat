using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ImageCropViewModel : BaseViewModel, IMvxViewModel<ImagePathNavigationParameter, ImageCropPathResult>
    {
        public string ImageFilePath { get; private set; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public MvxAsyncCommand<string> SetResultPathCommand { get; }

        public MvxAsyncCommand CancelCommand { get; }

        public ImageCropViewModel(INavigationService navigationService,
                         IErrorHandleService errorHandleService,
                         IApiService apiService,
                         IDialogService dialogService,
                         ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            SetResultPathCommand = new MvxAsyncCommand<string>(OnSetResultPath);
            CancelCommand = new MvxAsyncCommand(OnCancel);
        }

        public void Prepare(ImagePathNavigationParameter parameter)
        {
            ImageFilePath = parameter.FilePath;
        }

        private Task OnSetResultPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Task.CompletedTask;

            CloseCompletionSource.SetResult(new ImageCropPathResult(filePath));
            return NavigationService.CloseView(this);
        }

        private Task OnCancel()
        {
            if (CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            return NavigationService.CloseView(this);
        }
    }
}
