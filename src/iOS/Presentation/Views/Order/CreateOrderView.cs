using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Order
{
    [MvxTabPresentation(TabName = "Create Order", TabIconName = ImageNames.IconUnselected, TabSelectedIconName = ImageNames.IconSelected, WrapInNavigationController = true)]
    public partial class CreateOrderView : BaseTabbedView<CreateOrderViewModel>
    {
        private const int MinimumDescriptionHeight = 80;

        private UIImage _checkedImage;
        private UIImage _uncheckedImage;

        private UITextPosition _position;
        private UITextView _dynamicDescriptionTextView;
        private UIBarButtonItem _notificationBarItem;
        private NSRange _privacyLinkRange;

        public string OrderDescription
        {
            set
            {
                if (value is null)
                {
                    return;
                }

                var size = GetTextViewHeight(descriptionTextView.Bounds.Width, descriptionTextView.Font, value);
                TextViewHeightConstraint.Constant = size > MinimumDescriptionHeight ? size : MinimumDescriptionHeight;
                descriptionPlaceholderLabel.Hidden = value.Length > 0;
                descriptionTopFloatingPlaceholderLabel.Hidden = value.Length == 0;
            }
        }

        public override bool CanHandleKeyboardNotifications => true;

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            base.OnKeyboardChanged(visible, keyboardHeight);

            var window = UIApplication.SharedApplication.KeyWindow;
            var bottomPadding = window.SafeAreaInsets.Bottom;
            var topPadding = window.SafeAreaInsets.Top;
            scrollViewBottomConstraint.Constant = visible ? -(keyboardHeight - (topPadding + bottomPadding)) : 0;
            UIView.Animate(0.5, () => View.LayoutIfNeeded());
        }

        protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<CreateOrderView, CreateOrderViewModel>();

            bindingSet.Bind(nameTextField).To(vm => vm.Title);
            bindingSet.Bind(descriptionTextView).To(vm => vm.Description);
            bindingSet.Bind(this).For(nameof(OrderDescription)).To(vm => vm.Description);
            bindingSet.Bind(priceTextField).To(vm => vm.Price)
               .WithConversion<PriceConverter>();
            bindingSet.Bind(completeDateTextField).To(vm => vm.ActiveFor.Title);
            bindingSet.Bind(completeDateTextField.Tap()).For(v => v.Command).To(vm => vm.ShowDateDialogCommand);
            bindingSet.Bind(createButton).To(vm => vm.CreateCommand);
            bindingSet.Bind(progressBarView).For(v => v.BindVisible()).To(vm => vm.IsBusy);
            bindingSet.Bind(_notificationBarItem).For(v => v.Image).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications)
               .WithConversion<BoolToNotificationImageConverter>();
            bindingSet.Bind(HideExecutorCheckBoxButton).For(v => v.IsChecked).To(vm => vm.IsExecutorHidden)
                .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay);
            bindingSet.Bind(InfoImageView).For(v => v.BindTap()).To(vm => vm.ShowWalkthrouthSecretCommand);
		}

        private nfloat GetTextViewHeight(double width, UIFont font, string text)
        {
            _dynamicDescriptionTextView.Frame = new CoreGraphics.CGRect(0, 0, width, double.MaxValue);
            _dynamicDescriptionTextView.Font = font;
            _dynamicDescriptionTextView.Text = text;
            _dynamicDescriptionTextView.SizeToFit();

            return _dynamicDescriptionTextView.Frame.Height;
        }

        protected override void SetupControls()
		{
            rootView.AddGestureRecognizer(new UITapGestureRecognizer(OnViewTapped));

            descriptionTextView.TextContainer.MaximumNumberOfLines = 100;
            descriptionContainerView.Layer.BorderColor = Theme.Color.TextFieldDarkBorder.CGColor;
            descriptionContainerView.Layer.BorderWidth = 1f;
            descriptionContainerView.Layer.CornerRadius = 3f;
            _dynamicDescriptionTextView = new UITextView();
            DefinesPresentationContext = true;

            _notificationBarItem = NavigationItemHelper.CreateBarButton(ImageNames.IconNotification, ViewModel.ShowNotificationCommand);
            NavigationItem?.SetRightBarButtonItems(new UIBarButtonItem[]
            {
                _notificationBarItem,
                NavigationItemHelper.CreateBarButton(ImageNames.IconInfo, ViewModel.ShowWalkthrouthCommand)
            }, true);

            _checkedImage = UIImage.FromBundle(ImageNames.IconChecked);
            _uncheckedImage = UIImage.FromBundle(ImageNames.IconUnchecked);

            Title = Resources.CreateOrderView_Title;

            nameTextField.SetDarkStyle(Resources.CreateOrderView_Name_Placeholder);

            descriptionTextView.SetTitleStyle(size:14);
            descriptionTextView.ContentInset = UIEdgeInsets.Zero;
            descriptionTextView.ShouldChangeText = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= Constants.Orders.DescriptionMaxLength;
            };

            descriptionTextView.AddGestureRecognizer(new UITapGestureRecognizer(() => descriptionTextView.BecomeFirstResponder()));

            descriptionPlaceholderLabel.SetSmallSubtitleStyle(Resources.CreateOrderView_Description_Placeholder, 14);
            descriptionTopFloatingPlaceholderLabel.SetSmallSubtitleStyle(Resources.CreateOrderView_Description_Placeholder);
            descriptionTopFloatingPlaceholderLabel.Hidden = true;

            priceTextField.SetDarkStyle(Resources.CreateOrderView_Price_Placeholder, rightPadding: 14);
            priceTextField.TextAlignment = UITextAlignment.Right;

            completeDateTextField.SetDarkStyle(Resources.CreateOrderView_CompleteDate_Placeholder, rightImage: UIImage.FromBundle(ImageNames.IconCalendarAccent));

            hideExecutorCheckboxLabel.Text = Resources.Create_Order_Secret_order;
            hideExecutorCheckboxLabel.SetRegularStyle(14, Theme.Color.Black);
            hideExecutorCheckboxLabel.UserInteractionEnabled = true;
            hideExecutorCheckboxLabel.AddGestureRecognizer(new UITapGestureRecognizer(OnCheckboxTapped));

            createButton.SetDarkStyle(Resources.CreateOrderView_Create_Button);

            lottieAnimationView.SetAnimationNamed("Animations/ripple_animation");
            lottieAnimationView.LoopAnimation = true;
            lottieAnimationView.Play();

            stackView.SetCustomSpacing(8, stackView.ArrangedSubviews[0]);
            SetupPrivacyLabelAttributedText();
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(scrollView);
            views.Add(stackView);
            views.Add(descriptionTextView);

            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.Add(nameTextField);
            viewList.Add(descriptionTextView);
            viewList.Add(completeDateTextField);
            viewList.Add(priceTextField);

            descriptionTextView.ScrollEnabled = false;

            base.RegisterKeyboardDismissTextFields(viewList);
        }

        protected override void Subscription()
        {
            priceTextField.EditingChanged += OnPriceTextFieldEditingChanged;
        }

        protected override void Unsubscription()
        {
            priceTextField.EditingChanged -= OnPriceTextFieldEditingChanged;
        }

        private void OnViewTapped()
        {
            View.EndEditing(true);
        }

        private void OnPriceTextFieldEditingChanged(object sender, System.EventArgs e)
        {
            var text = priceTextField.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (text.EndsWith(Resources.Currency))
            {
                var position = priceTextField.GetPosition(priceTextField.EndOfDocument, -2);
                if (_position == position)
                {
                    return;
                }

                _position = position;
                priceTextField.SelectedTextRange = priceTextField.GetTextRange(_position, _position);
            }
        }

        private void OnCheckboxTapped()
        {
            HideExecutorCheckBoxButton.SwitchChecked();
        }

        private void SetupPrivacyLabelAttributedText()
        {
            var linkAttributes = new UIStringAttributes
            {
                UnderlineStyle = NSUnderlineStyle.Single,
                ForegroundColor = Theme.Color.Gray
            };

            privacyPolicyLabel.SetRegularStyle(10, Theme.Color.Gray);
            var privacyMessageAttributedString = new NSMutableAttributedString(Resources.Create_Order_Privacy_Message);

            var startPosition = Resources.Create_Order_Privacy_Message.IndexOf(Resources.Create_Order_Privacy_Link);
            _privacyLinkRange = new NSRange(startPosition, Resources.Create_Order_Privacy_Link.Length);
            privacyMessageAttributedString.AddAttributes(linkAttributes, _privacyLinkRange);

            privacyPolicyLabel.AttributedText = privacyMessageAttributedString;
            privacyPolicyLabel.AddGestureRecognizer(new UITapGestureRecognizer(OnPrivacyLabelTapped));
            privacyPolicyLabel.UserInteractionEnabled = true;
        }

        private void OnPrivacyLabelTapped(UITapGestureRecognizer gesture) =>
            ViewModel?.ShowPrivacyPolicyCommand?.Execute(null);
    }
}
