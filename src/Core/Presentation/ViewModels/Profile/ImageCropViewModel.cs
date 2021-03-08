using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Profile
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