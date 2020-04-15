using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.cashbox_tab_layout, ViewPagerResourceId = Resource.Id.cashbox_pager, ActivityHostViewModelType = typeof(CashboxViewModel))]
    [Register(nameof(RefillView))]
    public class RefillView : BaseFragment<RefillViewModel>
    {
        private EditText _priceEditText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.refill_layout, null);
            _priceEditText = view.FindViewById<EditText>(Resource.Id.refill_cost_edit_text);
            return view;
        }

        protected override void Subscription()
        {
            _priceEditText.TextChanged += PriceEditTextOnTextChanged;
        }

        protected override void Unsubscription()
        {
            _priceEditText.TextChanged -= PriceEditTextOnTextChanged;
        }

        private void PriceEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Presentation.Localization.Resources.Currency))
            {
                _priceEditText.SetSelection(text.Length - 2);
            }
        }
    }
}
