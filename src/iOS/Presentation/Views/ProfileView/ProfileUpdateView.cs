using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Binding;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Providers;
using System;
using System.Collections.Generic;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ProfileUpdateView : BaseTransparentBarView<ProfileUpdateViewModel>
    {
        private const int MinimumDescriptionHeight = 80;

        private UITextView _dynamicDescriptionTextView;
        private CAShapeLayer _fullBorderLayer;
        private CAShapeLayer _partBorderLayer;

        public override bool CanHandleKeyboardNotifications => true;

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            base.OnKeyboardChanged(visible, keyboardHeight);

            var window = UIApplication.SharedApplication.KeyWindow;
            var bottomPadding = window.SafeAreaInsets.Bottom;
            var topPadding = window.SafeAreaInsets.Top;
            scrollViewBottomConstraint.Constant = visible ? (keyboardHeight - (topPadding + bottomPadding)) : 0;
            UIView.Animate(0.5, () => View.LayoutIfNeeded());
        }

        public string UserDescription
        {
            set
            {
                if (value is null)
                {
                    descriptionContainerView.Layer.AddSublayer(_fullBorderLayer);
                    return;
                }

                var size = GetTextViewHeight(descriptionTextView.Bounds.Width, descriptionTextView.Font, value);
                textViewHeightConstraint.Constant = size > MinimumDescriptionHeight ? size : MinimumDescriptionHeight;
                descriptionPlaceholderLabel.Hidden = value.Length > 0;
                descriptionTopFloatingPlaceholderLabel.Hidden = value.Length == 0;

                if (value.Length > 0)
                {
                    _fullBorderLayer.Hidden = true;
                    _partBorderLayer.Hidden = false;
                    return;
                }

                _fullBorderLayer.Hidden = false;
                _partBorderLayer.Hidden = true;
            }
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<ProfileUpdateView, ProfileUpdateViewModel>();

            bindingSet.Bind(emailTextField).For(v => v.Text).To(vm => vm.Email)
                      .Mode(MvxBindingMode.TwoWay);
            bindingSet.Bind(loginTextField).For(v => v.Text).To(vm => vm.Login)
                      .Mode(MvxBindingMode.TwoWay);
            bindingSet.Bind(nameTextField).For(v => v.Text).To(vm => vm.Name)
                      .Mode(MvxBindingMode.TwoWay);
            bindingSet.Bind(birthdayTextField).For(v => v.Text).To(vm => vm.BirthdayText);
            bindingSet.Bind(birthdayTextField.Tap()).For(v => v.Command).To(vm => vm.SelectBirthdayCommand);
            bindingSet.Bind(progressBar).For(v => v.BindHidden()).To(vm => vm.IsBusy)
                      .WithConversion<MvxInvertedBooleanConverter>();
            bindingSet.Bind(saveButton).To(vm => vm.SaveProfileCommand);
            bindingSet.Bind(changePasswordLabel.Tap()).For(tap => tap.Command).To(vm => vm.ChangePasswordCommand);
            bindingSet.Bind(profileImage).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneWay);
            bindingSet.Bind(profileImage.Tap()).For(v => v.Command).To(vm => vm.ChangeProfilePhotoCommand);
            bindingSet.Bind(profileImage).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName);
            bindingSet.Bind(textLengthLabel).For(v => v.Text).To(vm => vm.LimitTextPresentation);
            bindingSet.Bind(changeProfilePhotoLabel.Tap()).For(v => v.Command).To(vm => vm.ChangeProfilePhotoCommand);
            bindingSet.Bind(femaleIconButton).To(vm => vm.SelectGenderCommand)
                      .CommandParameter(GenderType.Female);
            bindingSet.Bind(femaleIconButton).For(UIButtonSelectedTargetBinding.TargetBinding).To(vm => vm.IsGenderFemale);
            bindingSet.Bind(maleIconButton).To(vm => vm.SelectGenderCommand)
                      .CommandParameter(GenderType.Male);
            bindingSet.Bind(maleIconButton).For(UIButtonSelectedTargetBinding.TargetBinding).To(vm => vm.IsGenderMale);
            bindingSet.Bind(descriptionTextView).To(vm => vm.Description);
            bindingSet.Bind(this).For(nameof(UserDescription)).To(vm => vm.Description);
            bindingSet.Bind(emailValidationImageView).For(v => v.BindHidden()).To(vm => vm.IsEmailVerified);
            bindingSet.Bind(resndEmailContainerView).For(v => v.BindHidden()).To(vm => vm.IsEmailVerified)
                      .OneWay();
            bindingSet.Bind(emailValidationImageView).For(v => v.BindTap()).To(vm => vm.ShowValidationWarningCommand);
            bindingSet.Bind(resendEmailLabel).For(v => v.BindTap()).To(vm => vm.ResendEmailValidationCommand);
            bindingSet.Bind(emailTextField).For(FloatPlaceholderTextFieldPaddingTargetBinding.EndPadding).To(vm => vm.IsEmailVerified)
                      .WithConversion(BoolToIntConverter.Name, Tuple.Create(0, 45));
            bindingSet.Bind(resendEmailLabel).For(v => v.Alpha).To(vm => vm.CanResendEmailValidation)
                      .WithConversion(BoolToFloatConverter.Name, Tuple.Create(1f, 0.5f));
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            RefreshDescriptionFrameLayers();
        }

        protected override void SetupControls()
        {
            Title = Resources.ProfileUpdateView_Title;

            descriptionTextView.TextContainer.MaximumNumberOfLines = 3;

            _dynamicDescriptionTextView = new UITextView();

            textLengthLabel.SetRegularStyle(12, textLengthLabel.TextColor);

            rootView.AddGestureRecognizer(new UITapGestureRecognizer(OnViewTapped));
            NavigationItem?.SetRightBarButtonItem(NavigationItemHelper.CreateBarButton(ImagePathProvider.IconLogout, ViewModel.ShowMenuCommand), true);

            emailTextField.SetLightStyle(Resources.ProfileUpdateView_Email_Placeholder);

            loginTextField.SetLightStyle(Resources.ProfileUpdateView_Login_Placeholder);

            nameTextField.SetLightStyle(Resources.ProfileUpdateView_Name_Placeholder);

            birthdayTextField.SetLightStyle(Resources.ProfileUpdateView_Birthday_Placeholder, rightImage: UIImage.FromBundle(ImagePathProvider.IconCalendar));

            sexSelectTitleLabel.Text = Resources.ProfileUpdateView_GenderSelect_Title;
            sexSelectTitleLabel.TextColor = Theme.Color.White;
            sexSelectTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            maleTitleButton.SetTitle(Resources.RegistrationView_Male_Button, UIControlState.Normal);
            maleTitleButton.SetRadioTitleStyle();

            femaleTitleButton.SetTitle(Resources.RegistrationView_Female_Button, UIControlState.Normal);
            femaleTitleButton.SetRadioTitleStyle();

            femaleIconButton.SetSelectableImageStyle(ImagePathProvider.IconRadioButtonInactive, ImagePathProvider.IconRadioButtonActive);
            maleIconButton.SetSelectableImageStyle(ImagePathProvider.IconRadioButtonInactive, ImagePathProvider.IconRadioButtonActive);

            saveButton.SetLightStyle(Resources.ProfileUpdateView_Button_Save);
            resendEmailLabel.SetLinkStyle(Theme.Color.White, Resources.Profile_Resend_Confirmation, 14);

            changePasswordLabel.AttributedText = new NSAttributedString(Resources.ProfileUpdateView_ChangePassword, underlineStyle: NSUnderlineStyle.Single);
            changePasswordLabel.TextColor = UIColor.White;

            changeProfilePhotoLabel.AttributedText = new NSAttributedString(Resources.ProfileUpdateView_PhotoChange_Title, underlineStyle: NSUnderlineStyle.Single);
            changeProfilePhotoLabel.TextColor = UIColor.White;

            descriptionTextView.SetTitleStyle(size: 14);
            descriptionTextView.TextColor = Theme.Color.White;
            descriptionTextView.TintColor = Theme.Color.White;
            descriptionTextView.ContentInset = UIEdgeInsets.Zero;
            descriptionTextView.ShouldChangeText = (textField, range, replacementString) =>
            {
                if (replacementString == string.Empty)
                {
                    return true;
                }

                var newText = new NSString(textField.Text).Replace(range, new NSString(replacementString));
                var textWidth = textField.TextContainerInset.InsetRect(textField.Frame).Width;
                textWidth -= 2.0f * textField.TextContainer.LineFragmentPadding;

                var boundingRect = SizeOfString(newText, (float)textWidth, font: textField.Font);
                var numberOfLines = boundingRect.Height / textField.Font.LineHeight;
                var canChangeText = numberOfLines <= textField.TextContainer.MaximumNumberOfLines;
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return canChangeText && newLength <= Constants.Profile.DescriptionMaxLength;
            };

            descriptionTextView.AddGestureRecognizer(new UITapGestureRecognizer(() => descriptionTextView.BecomeFirstResponder()));

            descriptionPlaceholderLabel.SetSmallSubtitleStyle(Resources.ProfileUpdateView_Description_Placeholder, 14);
            descriptionPlaceholderLabel.TextColor = Theme.Color.White;

            descriptionTopFloatingPlaceholderLabel.SetSmallSubtitleStyle(Resources.ProfileUpdateView_Description_Placeholder);
            descriptionTopFloatingPlaceholderLabel.TextColor = Theme.Color.White;
            descriptionTopFloatingPlaceholderLabel.Hidden = true;
            stackView.SetCustomSpacing(8, stackView.ArrangedSubviews[2]);

            RefreshDescriptionFrameLayers();
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);
            views.Add(descriptionTextView);

            base.RegisterKeyboardDismissResponders(views);
        }

        private CGRect SizeOfString(string text, float width, UIFont font)
        {
            var attributes = new UIStringAttributes()
            {
                Font = font
            };

            return new NSString(text).GetBoundingRect(new CGSize(width, float.MaxValue),
                                                      NSStringDrawingOptions.UsesLineFragmentOrigin,
                                                      attributes,
                                                      null);
        }

        private void RefreshDescriptionFrameLayers()
        {
            var isHiddenFullPath = false;
            var isHiddenPartPath = false;

            if (_partBorderLayer != null)
            {
                isHiddenPartPath = _partBorderLayer.Hidden;
                isHiddenFullPath = _fullBorderLayer.Hidden;
                _partBorderLayer.RemoveFromSuperLayer();
                _fullBorderLayer.RemoveFromSuperLayer();
            }

            var partBrderPath = new CGPath();
            partBrderPath.MoveToPoint(0, 0);
            partBrderPath.AddLineToPoint(descriptionTopFloatingPlaceholderLabel.Frame.X, 0);
            partBrderPath.MoveToPoint(descriptionTopFloatingPlaceholderLabel.Frame.Right, 0);
            partBrderPath.AddLineToPoint(descriptionContainerView.Frame.Right, 0);
            partBrderPath.AddLineToPoint(descriptionContainerView.Frame.Right, descriptionContainerView.Frame.Height);
            partBrderPath.AddLineToPoint(0, descriptionContainerView.Frame.Height);
            partBrderPath.AddLineToPoint(0, 0);

            _partBorderLayer = new CAShapeLayer
            {
                Path = partBrderPath,
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = UIColor.White.CGColor,
                LineWidth = 1,
                Hidden = isHiddenPartPath
            };

            var fullBrderPath = new CGPath();
            fullBrderPath.MoveToPoint(0, 0);
            fullBrderPath.AddLineToPoint(descriptionContainerView.Frame.Right, 0);
            fullBrderPath.AddLineToPoint(descriptionContainerView.Frame.Right, descriptionContainerView.Frame.Height);
            fullBrderPath.AddLineToPoint(0, descriptionContainerView.Frame.Height);
            fullBrderPath.AddLineToPoint(0, 0);

            _fullBorderLayer = new CAShapeLayer
            {
                Path = fullBrderPath,
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = UIColor.White.CGColor,
                LineWidth = 1,
                Hidden = isHiddenFullPath
            };

            descriptionContainerView.Layer.AddSublayer(_fullBorderLayer);
            descriptionContainerView.Layer.AddSublayer(_partBorderLayer);
        }

        private nfloat GetTextViewHeight(double width, UIFont font, string text)
        {
            _dynamicDescriptionTextView.Frame = new CGRect(0, 0, width, double.MaxValue);
            _dynamicDescriptionTextView.Font = font;
            _dynamicDescriptionTextView.Text = text;
            _dynamicDescriptionTextView.SizeToFit();

            return _dynamicDescriptionTextView.Frame.Height;
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.AddRange(new UIView[] {
                emailTextField,
                loginTextField,
                nameTextField,
                birthdayTextField,
                descriptionTextView
            });

            descriptionTextView.ScrollEnabled = false;

            base.RegisterKeyboardDismissTextFields(viewList);
        }

        private void OnViewTapped()
        {
            View.EndEditing(true);
        }
    }
}