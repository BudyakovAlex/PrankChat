using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxTabLayoutPresentation(
        TabLayoutResourceId = Resource.Id.cashbox_tab_layout,
        ViewPagerResourceId = Resource.Id.cashbox_pager,
        ActivityHostViewModelType = typeof(CashboxViewModel))]
    [Register(nameof(WithdrawalView))]
    public class WithdrawalView : BaseFragment<WithdrawalViewModel>
    {
        private TextInputEditText _costEditText;
        private TextInputEditText _cardEditText;

        public WithdrawalView() : base(Resource.Layout.withdrawal_layout)
        {
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            var availableAmountTextView = view.FindViewById<TextView>(Resource.Id.withdrawal_available_amount_text);
            availableAmountTextView.PaintFlags |= PaintFlags.UnderlineText;

            _costEditText = view.FindViewById<TextInputEditText>(Resource.Id.credit_cost_text_input_edit_text);
            _cardEditText = view.FindViewById<TextInputEditText>(Resource.Id.credit_card_text_input_edit_text);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Context.SetTheme(Resource.Style.Theme_PrankChat_Base_Dark);
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        protected override void Subscription()
        {
            _costEditText.TextChanged += PriceEditTextOnTextChanged;
            _cardEditText.TextChanged += CardEditTextOnTextChanged;
        }

        protected override void Unsubscription()
        {
            _costEditText.TextChanged -= PriceEditTextOnTextChanged;
            _cardEditText.TextChanged -= CardEditTextOnTextChanged;
        }

        private void PriceEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Presentation.Localization.Resources.Currency))
            {
                _costEditText.SetSelection(text.Length - 2);
            }
        }

        private void CardEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            _cardEditText.SetSelection(text.Length);
        }
    }
}
