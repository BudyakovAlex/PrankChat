using Android.App;
using Android.Content.PM;
using Android.OS;
using Google.Android.Material.Button;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Droid.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;
using Android.Views;

namespace PrankChat.Mobile.Droid.Views
{
    [ClearStackActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/Theme.PrankChat.Base.Maintanance")]
    public class MaintananceView : BaseView<MaintananceViewModel>
    {
        private MaterialButton _downloadButton;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            var decorView = Window.DecorView;
            decorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.HideNavigation
                | SystemUiFlags.Immersive
                | SystemUiFlags.ImmersiveSticky);

            base.OnCreate(bundle, Resource.Layout.activity_maintanance);
            RequestedOrientation = ScreenOrientation.FullSensor;
            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void SetViewProperties()
        {
            _downloadButton = FindViewById<MaterialButton>(Resource.Id.download_button);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<MaintananceView, MaintananceViewModel>();

            bindingSet.Bind(_downloadButton).For(v => v.BindClick()).To(vm => vm.OpenInBrowserCommand);
        }
    }
}