using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Arbitration
{
    public class ArbitrationItemViewHolder : CardViewHolder<ArbitrationOrderItemViewModel>
    {
        private View _view;
        private View _backgroundView;
        private CircleCachedImageView _profilePhotoView;
        private TextView _titleTextView;
        private TextView _timeValueTextView;
        private TextView _priceValueTextView;
        private MaterialButton _actionButton;

        public ArbitrationItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _view = view;
            _profilePhotoView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_photo);
            _backgroundView = view.FindViewById<View>(Resource.Id.background_view);
            _titleTextView = view.FindViewById<TextView>(Resource.Id.order_title_text);
            _timeValueTextView = view.FindViewById<TextView>(Resource.Id.time_value_text);
            _priceValueTextView = view.FindViewById<TextView>(Resource.Id.price_value_text);
            _actionButton = view.FindViewById<MaterialButton>(Resource.Id.task_button);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<ArbitrationItemViewHolder, ArbitrationOrderItemViewModel>();

            bindingSet.Bind(_view).For(v => v.BindClick()).To(vm => vm.OpenDetailsOrderCommand);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.OrderTitle);
            bindingSet.Bind(_profilePhotoView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profilePhotoView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_profilePhotoView).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_priceValueTextView).For(v => v.Text).To(vm => vm.PriceText);
            bindingSet.Bind(_timeValueTextView).For(v => v.Text).To(vm => vm.TimeText);

            bindingSet.Bind(_actionButton).For(v => v.BindClick()).To(vm => vm.OpenDetailsOrderCommand);
            bindingSet.Bind(_actionButton).For(OrderButtonStyleBinding.TargetBinding).To(vm => vm.OrderType);

            bindingSet.Bind(_backgroundView).For(v => v.BindDrawable()).To(vm => vm.OrderType)
                      .WithConversion<OrderTypeToViewBackgroundConverter>();
        }
    }
}