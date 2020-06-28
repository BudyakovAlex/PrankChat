using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [ClearStackActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class MaintananceView : BaseView<MaintananceViewModel>
    {
        private Button _downloadButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_maintanance);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void SetViewProperties()
        {
            _downloadButton = FindViewById<Button>(Resource.Id.download_button);
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<MaintananceView, MaintananceViewModel>();

            bindingSet.Bind(_downloadButton)
                      .For(v => v.BindClick())
                      .To(vm => vm.OpenInBrowserCommand);

            bindingSet.Apply();
        }
    }
}