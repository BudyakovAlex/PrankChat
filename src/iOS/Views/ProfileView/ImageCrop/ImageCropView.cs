using CoreGraphics;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.iOS.Views.Base;
using PrankChat.Mobile.iOS.Views.ProfileView.ImageCrop;
using UIKit;
using Xam.Plugins.ImageCropper.iOS;

namespace PrankChat.Mobile.iOS.Views.ProfileView
{
    [MvxModalPresentation]
    public partial class ImageCropView : BaseViewController<ImageCropViewModel>
    {
        private TOCropViewController _pickerViewController;

        protected override void SetupControls()
        {
            var filePath = ViewModel.ImageFilePath;
            var image = UIImage.FromFile(filePath);

            var selector = new CropViewDelegate(ViewModel);
            var safeArea = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;
            _pickerViewController = new TOCropViewController(TOCropViewCroppingStyle.Circular, image);
            _pickerViewController.Delegate = selector;
            _pickerViewController.View.Frame =
                new CGRect(
                    new CGPoint(0, safeArea.Top),
                    new CGSize(View.Bounds.Size.Width, View.Bounds.Size.Height - safeArea.Top - safeArea.Bottom));
            View.BackgroundColor = _pickerViewController.View.BackgroundColor;
            View.Add(_pickerViewController.View);
        }
    }
}

