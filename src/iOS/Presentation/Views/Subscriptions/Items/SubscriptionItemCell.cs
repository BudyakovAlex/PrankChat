using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions.Items;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Subscriptions.Items
{
    public partial class SubscriptionItemCell : BaseTableCell<SubscriptionItemCell, SubscriptionItemViewModel>
    {
        public const float Height = 56f;

        protected SubscriptionItemCell(IntPtr handle)
            : base(handle)
        {
        }

        protected override void Bind()
        {
            base.Bind();

            var bindingSet = this.CreateBindingSet<SubscriptionItemCell, SubscriptionItemViewModel>();

            bindingSet.Bind(this).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(AvatarImageView).For(v => v.ImagePath).To(vm => vm.Avatar);
            bindingSet.Bind(AvatarImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortLogin);
            bindingSet.Bind(NameLabel).For(v => v.Text).To(vm => vm.Login);
            bindingSet.Bind(DescriptionLabel).For(v => v.Text).To(vm => vm.Description);

            bindingSet.Apply();
        }
    }
}
