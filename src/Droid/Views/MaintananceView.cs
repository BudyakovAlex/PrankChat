using Android.App;
using Android.Content.PM;
using Android.OS;
using Google.Android.Material.Button;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Droid.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Views
{
    [ClearStackActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class MaintananceView : BaseView<MaintananceViewModel>
    {
        private MaterialButton _downloadButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_maintanance);

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