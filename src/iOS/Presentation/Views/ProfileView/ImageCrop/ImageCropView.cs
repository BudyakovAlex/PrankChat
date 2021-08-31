using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Presentation.Views.ProfileView.ImageCrop;
using UIKit;
using Xam.Plugins.ImageCropper.iOS;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxModalPresentation]
    public partial class ImageCropView : BaseView<ImageCropViewModel>
    {
        private TOCropViewController _pickerViewController;

        protected override void SetupControls()
        {
            var filePath = ViewModel.ImageFilePath;
            var image = UIImage.FromFile(filePath);

            var selector = new CropViewDelegate(ViewModel);
            _pickerViewController = new TOCropViewController(TOCropViewCroppingStyle.Circular, image);
            _pickerViewController.Delegate = selector;
            _pickerViewController.View.Frame = new CoreGraphics.CGRect(View.Bounds.Location, View.Bounds.Size);
            View.Add(_pickerViewController.View);
        }
    }
}

