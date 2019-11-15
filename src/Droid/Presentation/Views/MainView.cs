using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [Activity(LaunchMode = LaunchMode.SingleTop)]
    public class MainView : BaseView<MainViewModel>
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.main_view_layout);

            if (bundle == null)
            {
                ViewModel.ShowContentCommand.Execute();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_view_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.notificationButton:
                    ViewModel.ShowNotificationCommand.Execute();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
