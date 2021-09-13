using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Results;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Profile
{
    public class ImageCropViewModel : BasePageViewModel<ImagePathNavigationParameter, ImageCropPathResult>
    {
        public ImageCropViewModel()
        {
            SetResultPathCommand = this.CreateCommand<string>(SetResultPath);
            CancelCommand = this.CreateCommand(Cancel);
        }

        public string ImageFilePath { get; private set; }

        public IMvxAsyncCommand<string> SetResultPathCommand { get; }

        public IMvxAsyncCommand CancelCommand { get; }

        public override void Prepare(ImagePathNavigationParameter parameter)
        {
            ImageFilePath = parameter.FilePath;
        }

        private Task SetResultPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Task.CompletedTask;
            }

            return NavigationManager.CloseAsync(this, new ImageCropPathResult(filePath));
        }

        private Task Cancel()
        {
            return NavigationManager.CloseAsync(this, null);
        }
    }
}