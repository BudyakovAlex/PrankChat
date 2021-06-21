﻿using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Walthroughs;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Walthroughs
{
    [MvxModalPresentation(ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext)]
    public partial class WalthroughView : BaseView<WalthroughViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();
            titleLabel.SetRegularStyle(20, Theme.Color.White);
            descriptionLabel.SetRegularStyle(16, Theme.Color.White);
        }

        protected override void SetupBinding()
        {
            var bindingSet = this.CreateBindingSet<WalthroughView, WalthroughViewModel>();

            bindingSet.Bind(titleLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Title);

            bindingSet.Bind(descriptionLabel)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(closeImageView)
                      .For(v => v.BindTap())
                      .To(vm => vm.CloseCommand);

            bindingSet.Apply();
        }
    }
}