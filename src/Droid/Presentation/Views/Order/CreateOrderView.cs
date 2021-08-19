using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxTabLayoutPresentation(
        TabLayoutResourceId = Resource.Id.tabs,
        ViewPagerResourceId = Resource.Id.viewpager,
        ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(CreateOrderView))]
    public class CreateOrderView : BaseTabFragment<CreateOrderViewModel>
    {
        private TextInputEditText _priceEditText;
        private TextInputEditText _descriptionEditText;
        private TextInputEditText _titleText;
        private TextInputEditText _descriptionText;
        private TextInputEditText _createOrderPriceEditText;
        private TextView _dateTextView;
        private CheckBox _createOrderCheckBox;
        private ImageView _descriptionImageViewImageView;
        private MaterialButton _createButton;
        private FrameLayout _frameLayout;

        public CreateOrderView() : base(Resource.Layout.fragment_create_order)
        {
            HasOptionsMenu = true;
        }

        protected override string TitleActionBar => Core.Localization.Resources.CreateOrderView_Title;

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _priceEditText = view.FindViewById<TextInputEditText>(Resource.Id.create_order_price_edit_text);
            _descriptionEditText = view.FindViewById<TextInputEditText>(Resource.Id.order_description_edit_text);
            _descriptionEditText.SetFilters(new[] { new InputFilterLengthFilter(Constants.Orders.DescriptionMaxLength) });
            _titleText = view.FindViewById<TextInputEditText>(Resource.Id.title_text);
            _descriptionText = view.FindViewById<TextInputEditText>(Resource.Id.order_description_edit_text);
            _createOrderPriceEditText = view.FindViewById<TextInputEditText>(Resource.Id.create_order_price_edit_text);
            _dateTextView = view.FindViewById<TextView>(Resource.Id.date_text);
            _createOrderCheckBox = view.FindViewById<CheckBox>(Resource.Id.create_order_check_box);
            _descriptionImageViewImageView = view.FindViewById<ImageView>(Resource.Id.description_image_view);
            _createButton = view.FindViewById<MaterialButton>(Resource.Id.create_button);
            _frameLayout = view.FindViewById<FrameLayout>(Resource.Id.frame_layout);
        }

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_titleText).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_descriptionText).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_createOrderPriceEditText).For(v => v.Text).To(vm => vm.Price).WithConversion<PriceConverter>();
            bindingSet.Bind(_dateTextView).For(v => v.Text).To(vm => vm.ActiveFor.Title);
            bindingSet.Bind(_dateTextView).For(v => v.BindClick()).To(vm => vm.ShowDateDialogCommand);
            bindingSet.Bind(_createOrderCheckBox).For(v => v.Checked).To(vm => vm.IsExecutorHidden);
            bindingSet.Bind(_descriptionImageViewImageView).For(v => v.BindClick()).To(vm => vm.ShowWalkthrouthSecretCommand);
            bindingSet.Bind(_createButton).For(v => v.BindClick()).To(vm => vm.CreateCommand);
            bindingSet.Bind(_frameLayout).For(v => v.BindVisible()).To(vm => vm.IsBusy);
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
            if (text.EndsWith(Core.Localization.Resources.Currency))
            {
                _priceEditText.SetSelection(text.Length - 2);
            }
        }
    }
}