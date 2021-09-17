using Android.App;
using Android.Content.PM;
using Android.OS;
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
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Order;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Order
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreateOrderView : BaseView<CreateOrderViewModel>
    {
        private TextInputEditText _priceEditText;
        private TextInputEditText _descriptionEditText;
        private TextInputEditText _titleEditText;
        private TextInputEditText _createOrderPriceEditText;
        private TextView _dateTextView;
        private CheckBox _createOrderCheckBox;
        private ImageView _descriptionImageView;
        private MaterialButton _createButton;
        private FrameLayout _createOrderFrameLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_create_order);
        }

        protected override string TitleActionBar => Core.Localization.Resources.CreateOrder;

        protected override void SetViewProperties()
        {
            _priceEditText = FindViewById<TextInputEditText>(Resource.Id.create_order_price_edit_text);
            _descriptionEditText = FindViewById<TextInputEditText>(Resource.Id.order_description_edit_text);
            _descriptionEditText.SetFilters(new[] { new InputFilterLengthFilter(Constants.Orders.DescriptionMaxLength) });
            _titleEditText = FindViewById<TextInputEditText>(Resource.Id.title_input_edit_text);
            _createOrderPriceEditText = FindViewById<TextInputEditText>(Resource.Id.create_order_price_edit_text);
            _dateTextView = FindViewById<TextView>(Resource.Id.date_text_view);
            _createOrderCheckBox = FindViewById<CheckBox>(Resource.Id.create_order_check_box);
            _descriptionImageView = FindViewById<ImageView>(Resource.Id.description_image_view);
            _createButton = FindViewById<MaterialButton>(Resource.Id.create_button);
            _createOrderFrameLayout = FindViewById<FrameLayout>(Resource.Id.animation_frame_layout);

            _createOrderPriceEditText.SetTextChangeListened((sequence) => _createOrderPriceEditText.MoveCursorBeforeSymbol(Core.Localization.Resources.Currency, sequence));
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_titleEditText).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_descriptionEditText).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_createOrderPriceEditText).For(v => v.Text).To(vm => vm.Price).WithConversion<PriceConverter>();
            bindingSet.Bind(_dateTextView).For(v => v.Text).To(vm => vm.ActiveFor.Title);
            bindingSet.Bind(_dateTextView).For(v => v.BindClick()).To(vm => vm.ShowDateDialogCommand);
            bindingSet.Bind(_createOrderCheckBox).For(v => v.Checked).To(vm => vm.IsExecutorHidden);
            bindingSet.Bind(_descriptionImageView).For(v => v.BindClick()).To(vm => vm.ShowWalkthrouthSecretCommand);
            bindingSet.Bind(_createButton).For(v => v.BindClick()).To(vm => vm.CreateCommand);
            bindingSet.Bind(_createOrderFrameLayout).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }
    }
}