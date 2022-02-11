using CoreGraphics;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Invites;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Extensions;
using PrankChat.Mobile.iOS.Views.Base;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Invites
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class InviteFriendView : BaseViewController<InviteFriendViewModel>
    {
        private const float EmailTextFieldHorizontalPadding = 14;

        public override bool CanHandleKeyboardNotifications => true;

        public bool HasError
        {
            set
            {
                if (value)
                {
                    EmailTextField.Layer.BorderColor = Theme.Color.LightStateErrorWarning.CGColor;
                    EmailTextField.TextColor = Theme.Color.LightStateErrorWarning;
                }
                else
                {
                    EmailTextField.Layer.BorderColor = Theme.Color.TextIconDisabled.CGColor;
                    EmailTextField.TextColor = Theme.Color.TextIconSecondary;
                }
            }
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            Title = Resources.InviteFriend;
            EmailTextField.Placeholder = Resources.InviteFriend;

            SendButton.SetDarkStyle(Resources.Send);

            InitializeEmailTextField();
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(this).For(nameof(HasError)).To(vm => vm.HasError);
            bindingSet.Bind(DescriptionLabel).For(v => v.BindAttributedText()).To(vm => vm.Description);
            bindingSet.Bind(EmailTextField).For(v => v.Text).To(vm => vm.Email);
            bindingSet.Bind(ErrorLabel).For(v => v.Text).To(vm => vm.ErrorMessage);
            bindingSet.Bind(SendButton).For(v => v.BindTouchUpInside()).To(vm => vm.SendCommand);
            bindingSet.Bind(LoadingView).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            base.OnKeyboardChanged(visible, keyboardHeight);

            var bottomInset = visible
                ? keyboardHeight
                : 0;
            ScrollView.ContentInset = new UIEdgeInsets(0, 0, bottomInset, 0);
        }

        private void InitializeEmailTextField()
        {
            EmailTextField.Layer.BorderWidth = 1;
            EmailTextField.Layer.CornerRadius = 4;

            EmailTextField.ShouldReturn = OnEmailTextFieldShouldReturn;

            EmailTextField.LeftView = CreateEmailTextFieldPaddingView();
            EmailTextField.LeftViewMode = UITextFieldViewMode.Always;

            EmailTextField.RightView = CreateEmailTextFieldPaddingView();
            EmailTextField.RightViewMode = UITextFieldViewMode.Always;
        }

        private bool OnEmailTextFieldShouldReturn(UITextField _)
        {
            EmailTextField.EndEditing(true);
            ViewModel.SendCommand.Execute();

            return true;
        }

        private UIView CreateEmailTextFieldPaddingView() =>
            new UIView(new CGRect(0, 0, EmailTextFieldHorizontalPadding, 0));
    }
}
