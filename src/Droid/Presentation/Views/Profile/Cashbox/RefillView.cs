using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
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
        private EditText _refillCostEditText;
        private MaterialButton _refillButton;
        private MvxGridView _refillMethodsGrid;

        public RefillView() : base(Resource.Layout.refill_layout)
        {
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _refillCostEditText = view.FindViewById<EditText>(Resource.Id.refill_cost_edit_text);
            _refillButton = view.FindViewById<MaterialButton>(Resource.Id.refill_button);
            _refillMethodsGrid = view.FindViewById<MvxGridView>(Resource.Id.refill_methods_collection);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_refillCostEditText).For(v => v.Text).To(vm => vm.Cost)
                      .WithConversion<PriceConverter>();

            bindingSet.Bind(_refillButton).For(v => v.BindClick()).To(vm => vm.RefillCommand);
            bindingSet.Bind(_refillMethodsGrid).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refillMethodsGrid).For(v => v.ItemClick).To(vm => vm.SelectionChangedCommand);
        }

        protected override void Subscription()
        {
            _refillCostEditText.TextChanged += PriceEditTextOnTextChanged;
        }

        protected override void Unsubscription()
        {
            _refillCostEditText.TextChanged -= PriceEditTextOnTextChanged;
        }

        private void PriceEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Localization.Resources.Currency))
            {
                _refillCostEditText.SetSelection(text.Length - 2);
            }
        }
    }
}
