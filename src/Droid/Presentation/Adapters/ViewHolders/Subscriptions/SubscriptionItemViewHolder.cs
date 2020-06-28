using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Subscriptions
{
    public class SubscriptionItemViewHolder : CardViewHolder
    {
        private CircleCachedImageView _avatarImageView;
        private TextView _nameTextView;
        private TextView _descriptionTextView;

        public SubscriptionItemViewHolder(View view, IMvxAndroidBindingContext context)
            : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _avatarImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.avatar_image_view);
            _nameTextView = view.FindViewById<TextView>(Resource.Id.name_text_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<SubscriptionItemViewHolder, SubscriptionItemViewModel>();

            bindingSet.Bind(_avatarImageView).For(v => v.BindClick()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(_avatarImageView).For(v => v.ImagePath).To(vm => vm.Avatar);
            bindingSet.Bind(_avatarImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortLogin);
            bindingSet.Bind(_nameTextView).For(v => v.Text).To(vm => vm.Login);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);

            bindingSet.Apply();
        }
    }
}
