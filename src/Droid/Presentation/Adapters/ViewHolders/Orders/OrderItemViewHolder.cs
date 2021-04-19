using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Presentation.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders
{
    public class OrderItemViewHolder : CardViewHolder<OrderItemViewModel>
    {
        private View _view;
        private ImageView _statusImageView;
        private View _backgroundView;
        private CircleCachedImageView _profilePhotoView;
        private TextView _titleTextView;
        private ImageView _hiddenOrderImageView;
        private TextView _startTimeTextView;
        private TextView _timeValueTextView;
        private TextView _dayTextView;
        private TextView _hoursTextView;
        private TextView _minutesTextView;
        private TextView _priceValueTextView;
        private TextView _taskStatusTextView;

        public OrderItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        private OrderTagType _orderTagType;
        public OrderTagType OrderTagType
        {
            get => _orderTagType;
            set
            {
                _orderTagType = value;

                var id = GetDrawableId(_orderTagType);
                _statusImageView.SetImageResource(id);
            }
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _view = view;
            _statusImageView = view.FindViewById<ImageView>(Resource.Id.status_image_view);
            _backgroundView = view.FindViewById<View>(Resource.Id.background_view);
            _profilePhotoView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_photo);
            _titleTextView = view.FindViewById<TextView>(Resource.Id.order_title_text);
            _hiddenOrderImageView = view.FindViewById<ImageView>(Resource.Id.image_hidden_order);
            _startTimeTextView = view.FindViewById<TextView>(Resource.Id.start_time_text);
            _timeValueTextView = view.FindViewById<TextView>(Resource.Id.time_value_text);
            _dayTextView = view.FindViewById<TextView>(Resource.Id.day_text);
            _hoursTextView = view.FindViewById<TextView>(Resource.Id.hours_text_view);
            _minutesTextView = view.FindViewById<TextView>(Resource.Id.minutes_text_view);
            _priceValueTextView = view.FindViewById<TextView>(Resource.Id.price_value_text);
            _taskStatusTextView = view.FindViewById<TextView>(Resource.Id.task_status_value_text);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<OrderItemViewHolder, OrderItemViewModel>();

            bindingSet.Bind(_view).For(v => v.BindClick()).To(vm => vm.OpenDetailsOrderCommand);
            bindingSet.Bind(_hiddenOrderImageView).For(v => v.BindVisible()).To(vm => vm.IsHiddenOrder);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_profilePhotoView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profilePhotoView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(_profilePhotoView).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_startTimeTextView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_priceValueTextView).For(v => v.Text).To(vm => vm.PriceText);
            bindingSet.Bind(_timeValueTextView).For(v => v.Text).To(vm => vm.TimeText);
            bindingSet.Bind(_taskStatusTextView).For(v => v.Text).To(vm => vm.StatusText);
            bindingSet.Bind(_timeValueTextView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);

            bindingSet.Bind(_dayTextView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_hoursTextView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);
            bindingSet.Bind(_minutesTextView).For(v => v.BindVisible()).To(vm => vm.IsTimeAvailable);

            bindingSet.Bind(this).For(v => v.OrderTagType).To(vm => vm.OrderTagType);
            bindingSet.Bind(_backgroundView).For(v => v.BindDrawable()).To(vm => vm.OrderType)
                      .WithConversion<OrderTypeToViewBackgroundConverter>();
        }

        private int GetDrawableId(OrderTagType orderTagType)
        {
            return orderTagType switch
            {
                OrderTagType.InModeration => Resource.Drawable.ic_order_tag_type_in_moderation,
                OrderTagType.New => Resource.Drawable.ic_order_tag_type_new,
                OrderTagType.NewNotMine => Resource.Drawable.ic_order_tag_type_new_not_mine,
                OrderTagType.Wait => Resource.Drawable.ic_order_tag_type_wait,
                OrderTagType.Finished => Resource.Drawable.ic_order_tag_type_finished,
                OrderTagType.InArbitration => Resource.Drawable.ic_order_tag_type_in_arbitration,
                OrderTagType.InWork => Resource.Drawable.ic_order_tag_type_in_work,
                _ => 0,
            };
        }
    }
}
