using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [MvxActivityPresentation]
    [Activity]
    public class MainView : BaseView
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_view_layout);
        }
    }
}
