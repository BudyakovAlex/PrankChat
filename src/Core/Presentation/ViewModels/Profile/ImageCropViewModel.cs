using System.Threading.Tasks;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ImageCropViewModel : BaseViewModel, IMvxViewModel<ImagePathNavigationParameter, ImageCropPathResult>
    {
        public ImageCropViewModel(INavigationService navigationService,
                                 IErrorHandleService errorHandleService,
                                 IApiService apiService,
                                 IDialogService dialogService,
                                 ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public string ImageFilePath { get; private set; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public void Prepare(ImagePathNavigationParameter parameter)
        {
            ImageFilePath = parameter.FilePath;
        }

        public void SetResultPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            CloseCompletionSource.SetResult(new ImageCropPathResult(filePath));
            NavigationService.CloseView(this).FireAndForget();
        }

        public void Cancel()
        {
            if (CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            NavigationService.CloseView(this).FireAndForget();
        }
    }
}
