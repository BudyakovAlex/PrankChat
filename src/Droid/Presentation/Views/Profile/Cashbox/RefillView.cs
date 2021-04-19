using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxTabLayoutPresentation(
        TabLayoutResourceId = Resource.Id.cashbox_tab_layout,
        ViewPagerResourceId = Resource.Id.cashbox_pager,
        ActivityHostViewModelType = typeof(CashboxViewModel))]
    [Register(nameof(RefillView))]
    public class RefillView : BaseFragment<RefillViewModel>
    {
        private EditText _priceEditText;

        public RefillView() : base(Resource.Layout.refill_layout)
        {
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _priceEditText = view.FindViewById<EditText>(Resource.Id.refill_cost_edit_text);
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
