using System;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Com.Theartofdev.Edmodo.Cropper;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Profile.ImageCrop
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class ImageCropView : BaseView<ImageCropViewModel>
    {
        private const int RotateAngle = 90;
        private CropImageView _cropImageView;

        protected override bool HasBackButton => true;

        public override void OnBackPressed()
        {
            ViewModel.CancelCommand.Execute();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_image_crop);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);

            _cropImageView = FindViewById<CropImageView>(Resource.Id.image_crop);
            var imagePath = ViewModel.ImageFilePath;
            var bitmap = BitmapFactory.DecodeFile(imagePath);
            _cropImageView.SetImageBitmap(bitmap);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.image_crop_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.image_crop_menu_rotate:
                    Rotate();
                    return true;

                case Resource.Id.image_crop_menu_accept:
                    Apply();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void Rotate()
        {
            _cropImageView.RotateImage(RotateAngle);
        }

        private void Apply()
        {
            var cropped = _cropImageView.CroppedImage;
            var path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, Guid.NewGuid().ToString());
            using (var fileStream = System.IO.File.Create(path))
            {
                cropped.Compress(Bitmap.CompressFormat.Png, 100, fileStream);
            }

            ViewModel.SetResultPathCommand.Execute(path);
        }
    }
}
