using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Search
{
    public partial class ProfileSearchItemCell : BaseTableCell<ProfileSearchItemCell, ProfileSearchItemViewModel>
    {
        static ProfileSearchItemCell()
        {
            EstimatedHeight = 56;
        }

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

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<ProfileSearchItemCell, ProfileSearchItemViewModel>();

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileDescriptionLabel)
                .To(vm => vm.ProfileDescription)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ImageUrl)
                .WithConversion<PlaceholderImageConverter>()
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortNameLabel)
                .To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortNameLabel)
                .For(v => v.BindHidden())
                .To(vm => vm.ImageUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Apply();
        }
    }
}

