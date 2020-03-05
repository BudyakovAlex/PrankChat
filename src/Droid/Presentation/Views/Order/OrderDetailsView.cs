using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxActivityPresentation]
    [Activity]
    public class OrderDetailsView : BaseView<OrderDetailsViewModel>
    {
        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.OrderDetailsView_Title;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_order_details_view);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.order_details_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings_menu_item:
                    ViewModel.OpenSettingsCommand.ExecuteAsync();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
