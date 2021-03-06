using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.ViewModels.Search.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Search
{
    public partial class ProfileSearchItemCell : BaseTableCell<ProfileSearchItemCell, ProfileSearchItemViewModel>
    {
        protected ProfileSearchItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            innerView.BackgroundColor = UIColor.Clear;

            profileNameLabel.SetSmallTitleStyle();
            profileDescriptionLabel.SetSmallSubtitleStyle();
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<ProfileSearchItemCell, ProfileSearchItemViewModel>();

            bindingSet.Bind(profileNameLabel).To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileDescriptionLabel).To(vm => vm.ProfileDescription)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.ImageUrl)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(this).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
        }
    }
}

