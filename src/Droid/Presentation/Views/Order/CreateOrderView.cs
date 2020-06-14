﻿using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(CreateOrderView))]
    public class CreateOrderView : BaseTabFragment<CreateOrderViewModel>
    {
        private TextInputEditText _priceEditText;
        private TextInputEditText _descriptionEditText;

        public CreateOrderView()
        {
            HasOptionsMenu = true;
        }

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.CreateOrderView_Title;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_create_order, null);
            _priceEditText = view.FindViewById<TextInputEditText>(Resource.Id.create_order_price_edit_text);
            _descriptionEditText = view.FindViewById<TextInputEditText>(Resource.Id.order_description_edit_text);
            _descriptionEditText.SetFilters(new[] { new InputFilterLengthFilter(Constants.Orders.DescriptionMaxLength) });
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
