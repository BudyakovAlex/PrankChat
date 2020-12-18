using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders
{
    public class OrderItemViewHolder : CardViewHolder<OrderItemViewModel>
    {
        private ImageView _statusImageView;
        private OrderTagType _orderTagType;

        public OrderItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

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

            _statusImageView = view.FindViewById<ImageView>(Resource.Id.status_image_view);
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<OrderItemViewHolder, OrderItemViewModel>();
            bindingSet.Bind(this).For(v => v.OrderTagType).To(vm => vm.OrderTagType);

            bindingSet.Apply();
        }

        private int GetDrawableId(OrderTagType orderTagType)
        {
            switch (orderTagType)
            {
                case OrderTagType.InModeration:
                    return Resource.Drawable.ic_order_tag_type_in_moderation;
                case OrderTagType.New:
                    return Resource.Drawable.ic_order_tag_type_new;
                case OrderTagType.NewNotMine:
                    return Resource.Drawable.ic_order_tag_type_new_not_mine;
                case OrderTagType.Wait:
                    return Resource.Drawable.ic_order_tag_type_wait;
                case OrderTagType.Finished:
                    return Resource.Drawable.ic_order_tag_type_finished;
                case OrderTagType.InArbitration:
                    return Resource.Drawable.ic_order_tag_type_in_arbitration;
                case OrderTagType.InWork:
                    return Resource.Drawable.ic_order_tag_type_in_work;
                case OrderTagType.None:
                default:
                    return 0;
            }
        }
    }
}
