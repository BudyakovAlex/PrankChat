using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.cashbox_tab_layout, ViewPagerResourceId = Resource.Id.cashbox_pager, ActivityHostViewModelType = typeof(CashboxViewModel))]
    [Register(nameof(WithdrawalView))]
    public class WithdrawalView : BaseFragment<WithdrawalViewModel>
    {
        private TextInputEditText _costEditText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Context.SetTheme(Resource.Style.Theme_PrankChat_Base_Dark);
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.withdrawal_layout, null);
            var availableAmountTextView = view.FindViewById<TextView>(Resource.Id.withdrawal_available_amount_text);
            availableAmountTextView.PaintFlags = availableAmountTextView.PaintFlags | PaintFlags.UnderlineText;

            _costEditText = view.FindViewById<TextInputEditText>(Resource.Id.credit_cost_text_input_edit_text);
            return view;
        }

        protected override void Subscription()
        {
            _costEditText.TextChanged += PriceEditTextOnTextChanged;
        }

        protected override void Unsubscription()
        {
            _costEditText.TextChanged -= PriceEditTextOnTextChanged;
        }

        private void PriceEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Presentation.Localization.Resources.Currency))
            {
                _costEditText.SetSelection(text.Length - 2);
            }
        }
    }
}
