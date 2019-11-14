using Android.App;
using Android.Content.PM;
using Android.OS;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [Activity(LaunchMode = LaunchMode.SingleTop)]
    public class MainView : BaseView<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_view_layout);

            if (bundle == null)
            {
                ViewModel.ShowContentCommand.Execute();
            }
        }
    }
}
