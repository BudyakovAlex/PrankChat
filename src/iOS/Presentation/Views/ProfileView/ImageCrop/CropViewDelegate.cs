using System;
using System.Diagnostics;
using System.IO;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using UIKit;
using Xam.Plugins.ImageCropper.iOS;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView.ImageCrop
{
    public class CropViewDelegate : TOCropViewControllerDelegate
    {
        private readonly ImageCropViewModel _viewModel;

        public bool DidCrop { get; private set; }

        public CropViewDelegate(ImageCropViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void DidCropToImage(TOCropViewController cropViewController, UIImage image, CoreGraphics.CGRect cropRect, nint angle)
        {
            DidCrop = true;

            try
            {
                if (image == null)
                {
                    Debug.WriteLine(this, "Image source seems unavailable.");
                    return;
                }

                var path = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, Guid.NewGuid().ToString());
                File.WriteAllBytes(path, image.AsPNG().ToArray());
                _viewModel.SetResultPath(path);
            }
            catch (Exception)
            {
                Debug.WriteLine("Image cropping failed.");
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }
        }

        public override void DidFinishCancelled(TOCropViewController cropViewController, bool cancelled)
        {
            _viewModel.Cancel();
        }
    }
}
