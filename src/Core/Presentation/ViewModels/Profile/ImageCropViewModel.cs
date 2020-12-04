using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
{
    public class ImageCropViewModel : BasePageViewModel, IMvxViewModel<ImagePathNavigationParameter, ImageCropPathResult>
    {
        public ImageCropViewModel()
        {
            SetResultPathCommand = new MvxAsyncCommand<string>(SetResultPath);
            CancelCommand = new MvxAsyncCommand(Cancel);
        }

        public string ImageFilePath { get; private set; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public IMvxAsyncCommand<string> SetResultPathCommand { get; }

        public IMvxAsyncCommand CancelCommand { get; }

        public void Prepare(ImagePathNavigationParameter parameter)
        {
            ImageFilePath = parameter.FilePath;
        }

        private Task SetResultPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Task.CompletedTask;
            }

            return NavigationService.CloseViewWithResult(this, new ImageCropPathResult(filePath));
        }

        private Task Cancel()
        {
            return NavigationService.CloseViewWithResult(this, null);
        }
    }
}